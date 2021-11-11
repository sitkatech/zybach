using System;
using System.Collections.Generic;
using GeoJSON.Net.Feature;

namespace Zybach.Models.DataTransferObjects
{
    public class StreamFlowZoneWellsDto
    {
        public StreamFlowZoneDto StreamFlowZone { get; set; }
        public List<WellWithIrrigatedAcresDto> Wells { get; set; }
    }


    public class WellSummaryDto
    {
        public string WellRegistrationID { get; set; }
        public string WellTPID { get; set; }
        public string Description { get; set; }
        public Feature Location { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public DateTime? FirstReadingDate { get; set; }
        public bool InAgHub { get; set; }
        public bool InGeoOptix { get; set; }
        public DateTime? FetchDate { get; set; }
        public bool HasElectricalData { get; set; }
        public List<IrrigatedAcresPerYearDto> IrrigatedAcresPerYear { get; set; }
        public string AgHubRegisteredUser { get; set; }
        public string FieldName { get; set; }
    }

    public class SensorSummaryDto
    {
        public string WellRegistrationID { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
        public bool IsActive{ get; set; }
    }

    public class WellWithSensorSummaryDto : WellSummaryDto
    {
        public List<SensorSummaryDto> Sensors { get; set; }
    }

    public class WellWithSensorMessageAgeDto : WellSummaryDto
    {
        public List<SensorMessageAgeDto> Sensors { get; set; }
    }

    public class WellDetailDto : WellWithSensorSummaryDto
    {
        public List<AnnualPumpedVolume> AnnualPumpedVolume { get; set; }
    }


    public class SensorMessageAgeDto
    {
        public string SensorName { get; set; }
        public int? MessageAge { get; set; }
        public string SensorType { get; set; }
        public bool IsActive { get; set; }
    }

    public class IrrigatedAcresPerYearDto
    {
        public int Year { get; set; }
        public double Acres { get; set; }
    }

    public class DailyPumpedVolume
    {
        public DailyPumpedVolume()
        {
        }

        public DailyPumpedVolume(DateTime time, double gallons, string dataSource)
        {
            Time = time;
            Gallons = gallons;
            DataSource = dataSource;
        }

        public DateTime Time { get; set; }
        public double? Gallons { get; set; }
        public string DataSource { get; set; }
        public string GallonsString => Gallons != null ? $"{Gallons:N1} gallons" : "N/A";
    }

    public class MonthlyPumpedVolume
    {
        public MonthlyPumpedVolume()
        {
        }

        public MonthlyPumpedVolume(int year, int month, double gallons)
        {
            Month = month;
            Year = year;
            VolumePumpedGallons = gallons;
        }

        public int Month { get; set; }
        public int Year { get; set; }
        public double VolumePumpedGallons { get; set; }
    }

    public class AnnualPumpedVolume
    {
        public AnnualPumpedVolume()
        {
        }

        public AnnualPumpedVolume(int year, double gallons, string dataSource)
        {
            Year = year;
            DataSource = dataSource;
            Gallons = gallons;
        }

        public int Year { get; set; }
        public string DataSource { get; set; }
        public double Gallons { get; set; }
    }

    public class InstallationRecordDto
    {
        public string InstallationCanonicalName { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string FlowMeterSerialNumber { get; set; }
        public string SensorSerialNumber { get; set; }
        public string Affiliation { get; set; }
        public string Initials { get; set; }
        public string SensorType { get; set; }
        public string ContinuitySensorModel { get; set; }
        public string FlowSensorModel { get; set; }
        public string PressureSensorModel { get; set; }
        public double? WellDepth { get; set; }
        public double? InstallDepth { get; set; }
        public double? CableLength { get; set; }
        public double? WaterLevel { get; set; }
        public List<string> Photos { get; set; }
    }
}