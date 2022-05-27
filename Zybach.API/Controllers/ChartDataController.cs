using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class ChartDataController : SitkaController<ChartDataController>
    {
        public ChartDataController(ZybachDbContext dbContext, ILogger<ChartDataController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("/api/chartData/{wellID}")]
        [ZybachViewFeature]
        public ActionResult<WellChartDataDto> GetInstallationRecordForWell([FromRoute] int wellID)
        {
            var well = Wells.GetByIDAsWellWithSensorSummaryDto(_dbContext, wellID);
            var wellChartDataDto = new WellChartDataDto();
            var sensors = well.Sensors;
            var hasElectricalData = well.HasElectricalData;

            var dailyPumpedVolumes = new List<DailyPumpedVolume>();

            var wellRegistrationID = well.WellRegistrationID;
            dailyPumpedVolumes.AddRange(GetDailyPumpedVolumeForWellAndSensorType(_dbContext, wellRegistrationID, sensors, MeasurementTypes.FlowMeter, MeasurementTypeEnum.FlowMeter));
            dailyPumpedVolumes.AddRange(GetDailyPumpedVolumeForWellAndSensorType(_dbContext, wellRegistrationID, sensors, MeasurementTypes.ContinuityMeter, MeasurementTypeEnum.ContinuityMeter));

            if (hasElectricalData)
            {
                var wellSensorMeasurementDtos = WellSensorMeasurements.GetWellSensorMeasurementsForWellByMeasurementType(_dbContext, wellRegistrationID, MeasurementTypeEnum.ElectricalUsage);
                var electricalBasedFlowEstimateSeries = CreateDailyPumpedVolumesAndZeroFillMissingDays(_dbContext, wellSensorMeasurementDtos, MeasurementTypes.ElectricalUsage);
                dailyPumpedVolumes.AddRange(electricalBasedFlowEstimateSeries);
            }

            wellChartDataDto.Sensors = sensors;
            wellChartDataDto.TimeSeries = dailyPumpedVolumes;

            return wellChartDataDto;
        }

        private IEnumerable<DailyPumpedVolume> GetDailyPumpedVolumeForWellAndSensorType(ZybachDbContext dbContext, string wellRegistrationID,
            IEnumerable<SensorSummaryDto> sensors, string sensorType, MeasurementTypeEnum measurementTypeEnum)
        {
            var sensorTypeSensors = sensors.Where(x => x.SensorType == sensorType).ToList();

            if (!sensorTypeSensors.Any())
            {
                return new List<DailyPumpedVolume>();
            }

            var wellSensorMeasurementDtos = WellSensorMeasurements.GetWellSensorMeasurementsForWellAndSensorsByMeasurementType(_dbContext, wellRegistrationID, measurementTypeEnum, sensorTypeSensors);
            return CreateDailyPumpedVolumesAndZeroFillMissingDays(dbContext, wellSensorMeasurementDtos, sensorType);
        }

        private static IEnumerable<DailyPumpedVolume> CreateDailyPumpedVolumesAndZeroFillMissingDays(ZybachDbContext dbContext, List<WellSensorMeasurementDto> wellSensorMeasurementDtos, string sensorType)
        {
            if (!wellSensorMeasurementDtos.Any())
            {
                return new List<DailyPumpedVolume>();
            }

            var anomalousDates = wellSensorMeasurementDtos
                .Where(x => x.SensorName != null)
                .Select(x => x.SensorName).Distinct()
                .ToDictionary(sensorName => sensorName, sensorName => SensorAnomalies.GetAnomolousDatesBySensorName(dbContext, sensorName));
            
            var measurementValues = wellSensorMeasurementDtos
                .Where(x => x.SensorName == null || !anomalousDates[x.SensorName].Contains(x.MeasurementDate))
                .ToLookup(x => x.MeasurementDate.ToShortDateString());

            var startDate = wellSensorMeasurementDtos.Min(x => x.MeasurementDateInPacificTime);
            var endDate = DateTime.Today;
            var list = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .ToList();
            var dailyPumpedVolumes = list.Select(a =>
            {
                var dateTime = startDate.AddDays(a);
                var gallons = measurementValues.Contains(dateTime.ToShortDateString()) ? measurementValues[dateTime.ToShortDateString()].Sum(x => x.MeasurementValue) : 0;
                return new DailyPumpedVolume(dateTime, gallons, sensorType);
            });
            return dailyPumpedVolumes;
        }
    }

    public class WellChartDataDto
    {
        public List<DailyPumpedVolume> TimeSeries { get; set; }
        public List<SensorSummaryDto> Sensors { get; set; }
    }
}