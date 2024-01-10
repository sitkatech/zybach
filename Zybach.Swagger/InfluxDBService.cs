using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.EFModels.Entities;

namespace Zybach.Swagger;

public class InfluxDBService
{
    private readonly ZybachSwaggerConfiguration _configuration;
    private readonly ILogger<InfluxDBService> _logger;
    private readonly InfluxDBClient _influxDbClient;

    public InfluxDBService(IOptions<ZybachSwaggerConfiguration> configuration, ILogger<InfluxDBService> logger)
    {
        _configuration = configuration.Value;
        _logger = logger;
        var options = new InfluxDBClientOptions.Builder()
            .Url(_configuration.INFLUXDB_URL)
            .AuthenticateToken(_configuration.INFLUXDB_TOKEN.ToCharArray())
            .TimeOut(TimeSpan.FromMinutes(10))
            .Build();
        _influxDbClient = InfluxDBClientFactory.Create(options);
    }

    public async Task<List<MeasurementReadingRaw>> GetWellPressureReadingsByRegistrationIDAndSensorName(string registrationID, string sensorName, DateTime startDate, DateTime endDate, string bucket)
    {
        var wellPressureMeasurementType = MeasurementType.WellPressure;

        var flux =
            $"from(bucket: \"{bucket}\")" +
            FilterByDateRange(startDate, endDate) +
            FilterByMeasurement(new List<string> { wellPressureMeasurementType.InfluxMeasurementName }) +
            FilterByRegistrationID(registrationID) +
            FilterBySensorName(new List<string> { sensorName });

        _logger.LogInformation($"Influx DB Query: {flux}");
        return await _influxDbClient.GetQueryApi().QueryAsync<MeasurementReadingRaw>(flux, _configuration.INFLUXDB_ORG);
    }

    public async Task WriteMeasurementReadingsToInfluxDb(List<MeasurementReadingRaw> measurementReadings, string bucket)
    {
        var points = measurementReadings.Select(x => PointData
                .Measurement(x.Measurement)
                .Timestamp(x.Time, WritePrecision.Ms)
                .Tag("eui", x.Eui)
                .Tag("registration-id", x.RegistrationID)
                .Tag("sn", x.Sensor)
                .Field(x.Field, x.Value))
            .ToList();

        var writeApiAsync = _influxDbClient.GetWriteApiAsync();
        await writeApiAsync.WritePointsAsync(points, bucket, _configuration.INFLUXDB_ORG);
    }

    public async Task DeletePressureSensorReadingsByRegistrationIDAndSensorName(string registrationID, string sensorName, DateTime startDate, DateTime endDate, string bucket)
    {
        var predicate = $"_measurement=\"depth\" AND \"registration-id\"=\"{registrationID}\" AND sn=\"{sensorName}\"";

        await _influxDbClient.GetDeleteApi().Delete(startDate, endDate, predicate, bucket, _configuration.INFLUXDB_ORG);
    }

    private static string FormatToZuluCentralTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddT05:00:00Z");
    }

    private static string FilterByDateRange(DateTime startDate, DateTime endDate)
    {
        return $"|> range(start: {FormatToZuluCentralTime(startDate)}, stop: {endDate:yyyy-MM-ddTHH:mm:ssZ}) ";
    }

    private static string FilterByRegistrationID(string registrationID)
    {
        return FilterByRegistrationID(new List<string> { registrationID });
    }

    private static string FilterByRegistrationID(List<string> registrationIDs)
    {
        return $"|> filter(fn: (r) => {string.Join(" or ", registrationIDs.Select(x => $"r[\"{FieldNames.RegistrationID}\"] == \"{x.ToLower()}\" or r[\"{FieldNames.RegistrationID}\"] == \"{x.ToUpper()}\") "))} ";
    }

    private static string FilterBySensorName(List<string> sensorNames)
    {
        return FilterByListImpl(FieldNames.SensorName, sensorNames);
    }

    private static string FilterByMeasurement(List<string> measurements)
    {
        return FilterByListImpl(FieldNames.Measurement, measurements);
    }

    private static string FilterByListImpl(string fieldName, IEnumerable<string> values)
    {
        return $"|> filter(fn: (r) => {string.Join(" or ", values.Select(x => $"r[\"{fieldName}\"] == \"{x}\""))}) ";
    }

    public class MeasurementReadingRaw
    {
        [Column(IsTimestamp = true)]
        public DateTime Time { get; set; }
        [Column(IsMeasurement = true)]
        public string Measurement { get; set; }
        [Column("value")]
        public double Value { get; set; }
        [Column("field")]
        public string Field { get; set; }
        [Column("eui", IsTag = true)]
        public string Eui { get; set; }
        [Column("registration-id", IsTag = true)]
        public string RegistrationID { get; set; }
        [Column("sn", IsTag = true)]
        public string Sensor { get; set; }
    }

    private struct FieldNames
    {
        public const string RegistrationID = "registration-id";
        public const string Measurement = "_measurement";
        public const string SensorName = "sn";
        public const string Field = "_field";
    }
}