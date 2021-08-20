using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services
{
    public class InfluxDBService
    {
        private readonly ZybachConfiguration _zybachConfiguration;
        private readonly ILogger<InfluxDBService> _logger;
        private readonly InfluxDBClient _influxDbClient;
        private readonly DateTime _defaultStartDate;

        public InfluxDBService(IOptions<ZybachConfiguration> zybachConfiguration, ILogger<InfluxDBService> logger)
        {
            _zybachConfiguration = zybachConfiguration.Value;
            _logger = logger;
            var options = new InfluxDBClientOptions.Builder()
                .Url(_zybachConfiguration.INFLUXDB_URL)
                .AuthenticateToken(_zybachConfiguration.INFLUXDB_TOKEN.ToCharArray())
                .ReadWriteTimeOut(TimeSpan.FromMinutes(10))
                .TimeOut(TimeSpan.FromMinutes(10))
                .Build();
            _influxDbClient = InfluxDBClientFactory.Create(options);
            _defaultStartDate = new DateTime(2000, 1, 1);
        }

        public async Task<List<WellSensorMeasurementStaging>> GetFlowMeterSeries(DateTime fromDate)
        {
            var flux = FilterByDateRange(fromDate, DateTime.Now) +
                       FilterByMeasurement(new List<string> {MeasurementNames.Gallons}) +
                       FilterByField(FieldNames.Pumped) +
                       GroupBy(new List<string> {FieldNames.RegistrationID, FieldNames.SensorName}) +
                       AggregateSumDaily(false);

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new WellSensorMeasurementStaging
            {
                WellRegistrationID = x.RegistrationID, ReadingDate = x.Time, MeasurementTypeID = (int) MeasurementTypeEnum.FlowMeter, MeasurementValue = x.Value, SensorName = x.Sensor
            }).ToList();
        }

        public async Task<List<WellSensorMeasurementStaging>> GetContinuityMeterSeries(DateTime fromDate)
        {
            var fluxQuery =
                "import \"math\" " +
                "import \"contrib/tomhollingworth/events\" " +
                $"from(bucket: \"{_zybachConfiguration.INFLUX_BUCKET}\") " +
                FilterByDateRange(fromDate, DateTime.Now) +
                FilterByMeasurement(new List<string> {MeasurementNames.Continuity}) +
                FilterByField(FieldNames.On) +
                GroupBy(new List<string> { FieldNames.RegistrationID, FieldNames.SensorName }) +
                "|> sort(columns: [\"_time\"]) " +
                "|> events.duration(unit: 1ns, columnName: \"run-time-ns\", timeColumn: \"_time\", stopColumn: \"_stop\") " +
                "|> filter(fn: (r) => r[\"_value\"] == 1) " +
                "|> map(fn: (r) => ({ r with \"run-time-minutes\": float(v: r[\"run-time-ns\"]) / 60000000000.0})) " +
                "|> map(fn: (r) => ({ r with \"_value\": math.mMin(x: r[\"run-time-minutes\"], y: 24.0 * 60.0)})) " +
                "|> aggregateWindow(every: 1d, fn: sum, createEmpty: false, timeSrc: \"_start\", column: \"_value\", offset: 5h) ";
            _logger.LogInformation($"Influx DB Query: {fluxQuery}");
            var fluxTables = await _influxDbClient.GetQueryApi().QueryAsync<MeasurementReading>(fluxQuery, _zybachConfiguration.INFLUXDB_ORG);

            return fluxTables.Select(x => new WellSensorMeasurementStaging
            {
                WellRegistrationID = x.RegistrationID, 
                ReadingDate = x.Time,
                MeasurementTypeID = (int) MeasurementTypeEnum.ContinuityMeter, 
                MeasurementValue = x.Value,
                SensorName = x.Sensor
            }).ToList();
        }


        public async Task<Dictionary<string, int>> GetLastMessageAgeBySensor()
        {
            var fluxQuery = "ageInSeconds = (x) => { " +
                            "timeNow = uint(v: now()) " +
                            "timeEvent = uint(v: x) " +
                            "return (timeNow - timeEvent) / uint(v: 1000000000)" +
                            "}" +
                            $"from(bucket: \"tpnrd\") " +
                            $"|> range(start: -30d) " +
                            FilterByMeasurement(new List<string> {MeasurementNames.IngestCount}) +
                            GroupBy(FieldNames.SensorName) +
                            "|> last() " +
                            "|> map(fn: (r) => ({ r with eventAge: ageInSeconds(x: r._time) }))";
            _logger.LogInformation($"Influx DB Query: {fluxQuery}");
            var fluxTables = await _influxDbClient.GetQueryApi().QueryAsync<MeasurementReading>(fluxQuery, _zybachConfiguration.INFLUXDB_ORG);
            return fluxTables.Where(x => !string.IsNullOrWhiteSpace(x.Sensor)).ToDictionary(x => x.Sensor.ToUpper(), registration => registration.EventAge);
        }

        // TODO: ported this but do we need this?
        public async Task<List<ResultFromInfluxDB>> GetFlowMeterSeries(List<string> wellRegistrationIDs, DateTime startDate, DateTime endDate, int interval)
        {
            var flux = FilterByDateRange(startDate, endDate) +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume }) +
                       FilterByField("gallons") +
                       FilterByRegistrationID(wellRegistrationIDs) + 
                       $"|> aggregateWindow(every: {interval}m, fn: sum, column: \"_value\", timeSrc: \"_stop\", timeDst: \"_time\", createEmpty: false ) " +
                       "|> sort(columns:[\"_time\"])";

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new ResultFromInfluxDB(x.Time, x.Value, x.RegistrationID)).ToList();
        }



        private async Task<List<MeasurementReading>> RunInfluxQueryAsync(string flux)
        {
            var fluxQuery = $"from(bucket: \"{_zybachConfiguration.INFLUX_BUCKET}\") {flux}";
            _logger.LogInformation($"Influx DB Query: {fluxQuery}");
            return await _influxDbClient.GetQueryApi().QueryAsync<MeasurementReading>(fluxQuery, _zybachConfiguration.INFLUXDB_ORG);
        }

        private static string AggregateSumDaily(bool createEmpty)
        {
            return $"|> aggregateWindow(every: 1d, fn: sum, createEmpty: {(createEmpty ? "true" : "false")}, timeSrc: \"_start\", offset: 5h)";
        }

        private string FilterByStartDate()
        {
            return $"|> range(start: {FormatToZuluCentralTime(_defaultStartDate)}) ";
        }

        private static string FormatToZuluCentralTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddT05:00:00Z");
        }

        private static string FilterByStartDate(DateTime startDate)
        {
            return $"|> range(start: {FormatToZuluCentralTime(startDate)}) ";
        }

        private static string FilterByYear(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);
            return FilterByDateRange(startDate, endDate);
        }

        private static string FilterByDateRange(DateTime startDate, DateTime endDate)
        {
            return $"|> range(start: {FormatToZuluCentralTime(startDate)}, stop: {endDate:yyyy-MM-ddTHH:mm:ssZ}) ";
        }

        private static string FilterByRegistrationID(string registrationID)
        {
            return FilterByRegistrationID(new List<string>{registrationID});
        }

        private static string FilterByRegistrationID(List<string> registrationIDs)
        {
            return $"|> filter(fn: (r) => {string.Join(" or ",registrationIDs.Select(x =>$"r[\"{FieldNames.RegistrationID}\"] == \"{x.ToLower()}\" or r[\"{FieldNames.RegistrationID}\"] == \"{x.ToUpper()}\") "))} ";
        }

        private static string FilterBySensorName(List<string> sensorNames)
        {
            return FilterByListImpl(FieldNames.SensorName, sensorNames);
        }

        private static string FilterByMeasurement(List<string> measurements)
        {
            return FilterByListImpl(FieldNames.Measurement, measurements);
        }

        private static string FilterByField(string fieldName)
        {
            return $"|> filter(fn: (r) => r[\"{FieldNames.Field}\"] == \"{fieldName}\" )";
        }

        private static string FilterByListImpl(string fieldName, IEnumerable<string> values)
        {
            return $"|> filter(fn: (r) => {string.Join(" or ", values.Select(x => $"r[\"{fieldName}\"] == \"{x}\""))}) ";
        }

        private static string GroupBy(string fieldName)
        {
            return GroupBy(new List<string> {fieldName});
        }

        private static string GroupBy(List<string> fieldNames)
        {
            return $"|> group(columns: [{string.Join(", ", fieldNames.Select(x => $"\"{x}\""))}]) ";
        }

        public class ResultFromInfluxDB
        {
            public ResultFromInfluxDB(DateTime endTime, double gallons, string wellRegistrationID)
            {
                EndTime = endTime;
                Gallons = gallons;
                WellRegistrationID = wellRegistrationID;
            }

            public ResultFromInfluxDB()
            {
            }

            public DateTime EndTime { get; set; }
            public double? Gallons { get; set; }
            public string WellRegistrationID { get; set; }
        }

        [Measurement("measurement")]
        private class MeasurementReading
        {
            [Column("registration-id")]
            public string RegistrationID { get; set; }
            [Column("sn")]
            public string Sensor { get; set; }
            [Column("_measurement")]
            public string Measurement { get; set; }
            [Column(IsTimestamp = true)]
            public DateTime Time { get; set; }
            [Column("_value")]
            public double Value { get; set; }
            [Column("eventAge")]
            public int EventAge { get; set; }
        }

        private struct MeasurementNames
        {
            public const string PumpedVolume = "pumped-volume";
            public const string EstimatedPumpedVolume = "estimated-pumped-volume";
            public const string IngestCount = "ingest-count";

            public const string Gallons = "gallons";
            public const string Continuity = "continuity";
        }

        private struct FieldNames
        {
            public const string RegistrationID = "registration-id";
            public const string Measurement = "_measurement";
            public const string SensorName = "sn";
            public const string On = "on";
            public const string Field = "_field";
            public const string Pumped = "pumped";
        }

        /// <summary>
        /// SensorTypes and DataSources are synonymous
        /// </summary>
        public struct SensorTypes
        {
            public const string ContinuityMeter = "Continuity Meter";
            public const string FlowMeter = "Flow Meter";
            public const string ElectricalUsage = "Electrical Usage";
        }
    }
}