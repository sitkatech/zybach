using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.Models.DataTransferObjects;

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
            _influxDbClient = InfluxDBClientFactory.Create(_zybachConfiguration.INFLUXDB_URL, _zybachConfiguration.INFLUXDB_TOKEN);
            _defaultStartDate = new DateTime(2000, 1, 1);
        }

        public async Task<Dictionary<string, DateTime>> GetFirstReadingDateTimesForSensor()
        {
            var flux = FilterByStartDate() +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume, MeasurementNames.EstimatedPumpedVolume }) +
                       GroupBy(FieldNames.SensorName) +
                       "|> first()";

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Where(x => !string.IsNullOrWhiteSpace(x.Sensor))
                .ToDictionary(x => x.Sensor.ToUpper(), x => x.Time);
        }

        public async Task<Dictionary<string, DateTime>> GetFirstReadingDateTimes()
        {
            var flux = FilterByStartDate() +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume, MeasurementNames.EstimatedPumpedVolume }) +
                       GroupBy(FieldNames.RegistrationID) +
                       "|> first()";

            var fluxTables = await RunInfluxQueryAsync(flux);
            var fluxTablesInAscendingOrder = fluxTables.OrderBy(x => x.Time).ToList();

            var output = new Dictionary<string, DateTime>();
            fluxTablesInAscendingOrder.ForEach(x =>
            {
                if (!output.ContainsKey(x.RegistrationID))
                {
                    output[x.RegistrationID] = x.Time;
                }
            });
            return output;
        }

        public async Task<Dictionary<string, DateTime>> GetLastReadingDateTimes()
        {
            var flux = FilterByStartDate() +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume, MeasurementNames.EstimatedPumpedVolume }) +
                       GroupBy(FieldNames.RegistrationID) +
                       "|> last()";

            var fluxTables = await RunInfluxQueryAsync(flux);

            var fluxTablesInDescendingOrder = fluxTables.OrderByDescending(x=>x.Time).ToList();

            var output = new Dictionary<string, DateTime>();
            fluxTablesInDescendingOrder.ForEach(x =>
            {
                if (!output.ContainsKey(x.RegistrationID))
                {
                    output[x.RegistrationID] = x.Time;
                }
            });
            return output;
        }

        public async Task<DateTime?> GetFirstReadingDateTimeForWell(string registrationID)
        {
            var flux = FilterByStartDate() +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume, MeasurementNames.EstimatedPumpedVolume }) +
                       FilterByRegistrationID(registrationID) +
                       "|> first()";

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Any() ? fluxTables.Min(x => x.Time) : (DateTime?) null;
        }

        public async Task<DateTime?> GetLastReadingDateTimeForWell(string registrationID)
        {
            var flux = FilterByStartDate() +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume, MeasurementNames.EstimatedPumpedVolume }) +
                       FilterByRegistrationID(registrationID) +
                       "|> last()";

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Any() ? fluxTables.Max(x => x.Time) : (DateTime?) null;
        }

        public async Task<List<DailyPumpedVolume>> GetPumpedVolumesForSensors(List<string> sensorNames, string sensorType, DateTime fromDate)
        {
            //TODO: talk with Nick on the offset
            //const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            //    millisecond: 0, minute: 0, second: 0, hour: 0
            //});
            var flux = FilterByStartDate(fromDate) +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume }) +
                       FilterBySensorName(sensorNames) +
                       GroupBy(FieldNames.RegistrationID) +
                       AggregateSumDaily();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new DailyPumpedVolume(x.Time, x.Value, sensorType)).ToList();
        }

        public async Task<List<MonthlyPumpedVolume>> GetMonthlyPumpedVolumesForSensors(List<string> sensorNames, string registrationID, DateTime fromDate)
        {
            //TODO: talk with Nick on the offset
            //const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            //    millisecond: 0, minute: 0, second: 0, hour: 0
            //});
            var flux = FilterByStartDate(fromDate) +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume }) +
                       FilterBySensorName(sensorNames) +
                       FilterByRegistrationID(registrationID) +
                       GroupBy(FieldNames.RegistrationID) +
                       AggregateSumMonthly();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new MonthlyPumpedVolume(x.Time, x.Value)).ToList();
        }

        public async Task<List<MonthlyPumpedVolume>> GetMonthlyElectricalBasedFlowEstimate(string registrationID, DateTime fromDate)
        {
            //TODO: talk with Nick on the offset
            //const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            //    millisecond: 0, minute: 0, second: 0, hour: 0
            //});
            var flux = FilterByStartDate(fromDate) +
                       FilterByMeasurement(new List<string> { MeasurementNames.EstimatedPumpedVolume }) +
                       FilterByRegistrationID(registrationID) +
                       AggregateSumMonthly();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new MonthlyPumpedVolume(x.Time, x.Value)).ToList();
        }

        public async Task<List<DailyPumpedVolume>> GetElectricalBasedFlowEstimateSeries(string registrationID, DateTime fromDate)
        {
            //TODO: talk with Nick on the offset
            //const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            //    millisecond: 0, minute: 0, second: 0, hour: 0
            //});
            var flux = FilterByStartDate(fromDate) +
                       FilterByMeasurement(new List<string> { MeasurementNames.EstimatedPumpedVolume }) +
                       FilterByRegistrationID(registrationID) +
                       AggregateSumDaily();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new DailyPumpedVolume(x.Time, x.Value, DataSources.ElectricalUsage)).ToList();
        }

        public async Task<List<AnnualPumpedVolume>> GetAnnualPumpedVolumesForSensor(List<string> sensorNames, string sensorType)
        {
            var flux = FilterByStartDate(new DateTime(2019, 1, 1)) +
                       FilterByMeasurement(new List<string> { MeasurementNames.PumpedVolume }) +
                       FilterBySensorName(sensorNames) +
                       GroupBy(FieldNames.RegistrationID) +
                       AggregateSumYearly();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new AnnualPumpedVolume(x.Time, x.Value, sensorType)).ToList();
        }

        public async Task<List<AnnualPumpedVolume>> GetAnnualEstimatedPumpedVolumeForWell(string registrationID)
        {
            var flux = FilterByStartDate(new DateTime(2019, 1, 1)) +
                       FilterByMeasurement(new List<string> { MeasurementNames.EstimatedPumpedVolume }) +
                       FilterByRegistrationID(registrationID) +
                       AggregateSumYearly();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => new AnnualPumpedVolume(x.Time, x.Value, DataSources.ElectricalUsage)).ToList();
        }

        public async Task<Dictionary<string, double>> GetAnnualEstimatedPumpedVolumeByWellForYear(int year)
        {
            var flux = FilterByYear(year) +
                       FilterByMeasurement(new List<string> { MeasurementNames.EstimatedPumpedVolume }) +
                       GroupBy(FieldNames.RegistrationID) +
                       AggregateSumYearly();

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.ToDictionary(x => x.RegistrationID.ToUpper(), x => x.Value);
        }

        public async Task<List<string>> GetWellRegistrationIDsWithElectricalEstimateAsOfYear(int year)
        {
            var flux = FilterByDateRange(new DateTime(2019, 1, 1), new DateTime(year + 1, 1, 1).AddMilliseconds(-1)) +
                       FilterByMeasurement(new List<string> { MeasurementNames.EstimatedPumpedVolume }) +
                       GroupBy(FieldNames.RegistrationID) +
                       "|> last()";

            var fluxTables = await RunInfluxQueryAsync(flux);
            return fluxTables.Select(x => x.RegistrationID.ToUpper()).ToList();
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

        private static string AggregateSumDaily()
        {
            return "|> aggregateWindow(every: 1d, fn: sum, createEmpty: true, timeSrc: \"_start\")";
        }

        private static string AggregateSumMonthly()
        {
            return "|> aggregateWindow(every: 1mo, fn: sum, createEmpty: true, timeSrc: \"_start\")";
        }

        private static string AggregateSumYearly()
        {
            return "|> aggregateWindow(every: 1y, fn: sum, createEmpty: true, timeSrc: \"_start\")";
        }

        private string FilterByStartDate()
        {
            return $"|> range(start: {_defaultStartDate:yyyy-MM-ddTHH:mm:ssZ}) ";
        }

        private static string FilterByStartDate(DateTime startDate)
        {
            return $"|> range(start: {startDate:yyyy-MM-ddTHH:mm:ssZ}) ";
        }

        private static string FilterByYear(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);
            return FilterByDateRange(startDate, endDate);
        }

        private static string FilterByDateRange(DateTime startDate, DateTime endDate)
        {
            return $"|> range(start: {startDate:yyyy-MM-ddTHH:mm:ssZ}, stop: {endDate:yyyy-MM-ddTHH:mm:ssZ}) ";
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
            return $"|> group(columns: [\"{fieldName}\"]) ";
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
        }

        private struct DataSources
        {
            public const string ElectricalUsage = "Electrical Usage";
        }

        private struct FieldNames
        {
            public const string RegistrationID = "registration-id";
            public const string Measurement = "_measurement";
            public const string SensorName = "sn";
            public const string Field = "_field";
        }
    }
}