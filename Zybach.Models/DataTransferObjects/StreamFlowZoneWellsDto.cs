﻿using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Zybach.Models.DataTransferObjects
{
    public class StreamFlowZoneWellsDto
    {
        public StreamFlowZoneDto StreamFlowZone { get; set; }
        public List<WellWithIrrigatedAcresDto> Wells { get; set; }
    }

    public class WellWaterLevelMapSummaryDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public object Location { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public string WellNickname { get; set; }
        public string TownshipRangeSection { get; set; }

        public List<SensorSummaryDto> Sensors { get; set; }
    }

    public class WellMinimalDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public object Location { get; set; }
        public List<SensorSummaryDto> Sensors { get; set; }
    }

    public class WellSummaryDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public string WellTPID { get; set; }
        public string Description { get; set; }
        public object Location { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public DateTime? FirstReadingDate { get; set; }
        public bool InAgHub { get; set; }
        public bool InGeoOptix { get; set; }
        public DateTime? FetchDate { get; set; }
        public bool HasElectricalData { get; set; }
        public List<IrrigatedAcresPerYearDto> IrrigatedAcresPerYear { get; set; }
        public string AgHubRegisteredUser { get; set; }
        public string FieldName { get; set; }
        public string WellNickname { get; set; }
        public int? PageNumber { get; set; }
        public string OwnerName { get; set; }
        public string TownshipRangeSection { get; set; }
        public bool? HasWaterQualityInspections { get; set; }
        public bool? HasWaterLevelInspections { get; set; }
    }

    public class SensorSummaryDto
    {
        public string WellRegistrationID { get; set; }
        public int SensorID { get; set; }
        public string SensorName { get; set; }
        public int? SensorTypeID { get; set; }
        public string SensorType { get; set; }
        public bool IsActive{ get; set; }
    }

    public class WellWithSensorSummaryDto : WellSummaryDto
    {
        public List<SensorSummaryDto> Sensors { get; set; }
        public bool WellConnectedMeter { get; set; }
    }

    public class WellWithSensorMessageAgeDto : WellSummaryDto
    {
        public List<SensorMessageAgeDto> Sensors { get; set; }
    }

    public class WellDetailDto : WellWithSensorSummaryDto
    {
        public int? AgHubIrrigationUnitID { get; set; }
        public List<AnnualPumpedVolume> AnnualPumpedVolume { get; set; }
        public string County { get; set; }
        public int? WellParticipationID { get; set; }
        public string WellParticipationName { get; set; }
        public int? WellUseID { get; set; }
        public string WellUseName { get; set; }
        public bool RequiresChemigation { get; set; }
        public bool RequiresWaterLevelInspection { get; set; }
        public decimal? WellDepth { get; set; }
        public string Clearinghouse { get; set; }
        public string SiteName { get; set; }
        public string SiteNumber { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerState { get; set; }
        public string OwnerZipCode { get; set; }
        public string AdditionalContactName { get; set; }
        public string AdditionalContactAddress { get; set; }
        public string AdditionalContactCity { get; set; }
        public string AdditionalContactState { get; set; }
        public string AdditionalContactZipCode { get; set; }
        public bool IsReplacement { get; set; }
        public string Notes { get; set; }
        public string WaterQualityInspectionTypes { get; set; }
        public string ScreenInterval { get; set; }
        public decimal? ScreenDepth { get; set; }
        public string IrrigationUnitGeoJSON { get; set; }
    }


    public class SensorMessageAgeDto
    {
        public int SensorID { get; set; }
        public string SensorName { get; set; }
        public int? MessageAge { get; set; }
        public double? LastVoltageReading { get; set; }
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