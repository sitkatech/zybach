using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Zybach.Models.DataTransferObjects;
using Point = GeoJSON.Net.Geometry.Point;
using Position = GeoJSON.Net.Geometry.Position;

namespace Zybach.EFModels.Entities
{
    public class Wells
    {
        public static WellDto GetByIDAsDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            return well?.AsDto();
        }

        public static WellSimpleDto GetByIDAsSimpleDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            return well?.AsSimpleDto();
        }

        public static List<Well> List(ZybachDbContext dbContext)
        {
            return GetWellsImpl(dbContext)
                .OrderBy(x => x.WellRegistrationID)
                .ToList();
        }

        public static List<WellWithSensorSimpleDto> ListAsWellWithSensorSimpleDto(ZybachDbContext dbContext)
        {
            var wellsWithWaterLevelInspections = dbContext.WaterLevelInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, y => y.Any());
            
            var wellsWithWaterQualityInspections = dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, y => y.Any());
            
            return GetWellsImpl(dbContext)
                .OrderBy(x => x.WellRegistrationID)
                .Select(x => WellWithSensorSimpleDtoFromWell(x,
                    wellsWithWaterLevelInspections.ContainsKey(x.WellID)
                        ? wellsWithWaterLevelInspections[x.WellID]
                        : null,
                    wellsWithWaterQualityInspections.ContainsKey(x.WellID)
                        ? wellsWithWaterQualityInspections[x.WellID]
                        : null))
                .ToList();
        }

        public static WellWithSensorSimpleDto GetByIDAsWellWithSensorSimpleDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            if (well != null)
            {
                return WellWithSensorSimpleDtoFromWell(well, null, null);
            }
            return null;
        }

        public static Well GetByWellRegistrationID(ZybachDbContext dbContext, string wellRegistrationID)
        {
            return dbContext.Wells.AsNoTracking().SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
        }

        public static Well GetByWellRegistrationIDWithTracking(ZybachDbContext dbContext, string wellRegistrationID)
        {
            return dbContext.Wells.SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
        }

        public static Well GetByID(ZybachDbContext dbContext, int wellID)
        {
            return GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
        }

        public static Well GetByIDWithTracking(ZybachDbContext dbContext, int wellID)
        {
            return dbContext.Wells.SingleOrDefault(x => x.WellID == wellID);
        }

        private static IQueryable<Well> GetWellsImpl(ZybachDbContext dbContext)
        {
            return dbContext.Wells
                .Include(x => x.AgHubWell)
                    .ThenInclude(x => x.AgHubIrrigationUnit)
                    .ThenInclude(x => x.AgHubIrrigationUnitGeometry)
                .Include(x => x.GeoOptixWell)
                .Include(x => x.AgHubWell.AgHubWellIrrigatedAcres)
                .Include(x => x.Sensors)
                    .ThenInclude(x => x.SensorAnomalies)
                .Include(x => x.Sensors)
                    .ThenInclude(x => x.SupportTickets)
                .Include(x => x.WellWaterQualityInspectionTypes)
                .Include(x => x.WellParticipation)
                .Include(x => x.WellGroupWells)
                    .ThenInclude(x => x.WellGroup)
                .AsNoTracking();
        }

        private static WellWithSensorSimpleDto WellWithSensorSimpleDtoFromWell(Well well, bool? hasWaterLevelInspections, bool? hasWaterQualityInspections)
        {
            var wellWithSensorSimpleDto = new WellWithSensorSimpleDto
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X))),
                FetchDate = well.LastUpdateDate,
                InGeoOptix = well.GeoOptixWell != null,
                InAgHub = well.AgHubWell != null,
                WellNickname = well.WellNickname,
                OwnerName = well.OwnerName,
                PageNumber = well.PageNumber,
                Notes = well.Notes,
                TownshipRangeSection = well.TownshipRangeSection,
                HasWaterLevelInspections = hasWaterLevelInspections,
                HasWaterQualityInspections = hasWaterQualityInspections
            };

            var sensors = well.Sensors.Select(x => x.AsSimpleDto()).ToList();

            var agHubWell = well.AgHubWell;
            if (agHubWell != null)
            {
                wellWithSensorSimpleDto.WellTPID = agHubWell.AgHubIrrigationUnit?.WellTPID;
                wellWithSensorSimpleDto.HasElectricalData = agHubWell.HasElectricalData;
                wellWithSensorSimpleDto.WellConnectedMeter = agHubWell.WellConnectedMeter;
                wellWithSensorSimpleDto.AgHubRegisteredUser = agHubWell.AgHubRegisteredUser;
                wellWithSensorSimpleDto.FieldName = agHubWell.FieldName;
                wellWithSensorSimpleDto.IrrigatedAcresPerYear = agHubWell.AgHubWellIrrigatedAcres
                    .Select(x => new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();
            }
            else
            {
                wellWithSensorSimpleDto.WellConnectedMeter = false;
                wellWithSensorSimpleDto.HasElectricalData = false;
                wellWithSensorSimpleDto.InAgHub = false;
            }

            wellWithSensorSimpleDto.Sensors = sensors;

            return wellWithSensorSimpleDto;
        }

        public static List<SearchSummaryDto> SearchByWellRegistrationID(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.WellRegistrationID.Contains(searchText))
                .Select(x => new SearchSummaryDto()
                {
                    ObjectType = "Well",
                    WellName = x.WellRegistrationID,
                    WellID = x.WellID
                }).ToList();
        }

        public static List<string> SearchByWellRegistrationIDHasInspectionType(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.WellWaterQualityInspectionTypes
                .Include(x => x.Well)
                .AsNoTracking()
                .Where(x => x.Well.WellRegistrationID.Contains(searchText))
                .Select(x => x.Well.WellRegistrationID).Distinct()
                .ToList();
        }

        public static List<string> SearchByWellRegistrationIDRequiresChemigation(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells
                .AsNoTracking()
                .Where(x => x.RequiresChemigation && x.WellRegistrationID.Contains(searchText))
                .Select(x => x.WellRegistrationID)
                .Distinct().ToList();
        }

        public static List<SearchSummaryDto> SearchByAghubRegisteredUser(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking()
                .Where(x => x.AgHubRegisteredUser.Contains(searchText))
                .Select(x => new SearchSummaryDto()
                {
                    ObjectName = x.AgHubRegisteredUser,
                    ObjectType = "Registered User",
                    WellName = x.Well.WellRegistrationID,
                    WellID = x.WellID
                }).ToList();
        }

        public static List<SearchSummaryDto> SearchByField(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking()
                .Where(x => x.FieldName.Contains(searchText))
                .Select(x => new SearchSummaryDto()
                {
                    ObjectName = x.FieldName,
                    ObjectType = "Field",
                    WellName = x.Well.WellRegistrationID,
                    WellID = x.WellID
                }).ToList();
        }

        public static List<SearchSummaryDto> SearchByChemigationPermit(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.ChemigationPermits.Include(x => x.Well).AsNoTracking().ToList()
                .Where(x => x.Well != null && x.ChemigationPermitNumberDisplay.Contains(searchText))
                .Select(x => new SearchSummaryDto()
                {
                    ObjectName = x.ChemigationPermitNumberDisplay,
                    ObjectType = "Chemigation Permit",
                    WellName = x.Well.WellRegistrationID,
                    WellID = x.WellID.Value
                }).ToList();
        }

        public static List<SearchSummaryDto> SearchBySensorName(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Sensors.Include(x => x.Well).AsNoTracking()
                .Where(x => x.Well != null && x.SensorName.Contains(searchText))
                .Select(x => new SearchSummaryDto()
                {
                    ObjectName = x.SensorName,
                    ObjectType = "Sensor",
                    WellName = x.Well.WellRegistrationID,
                    WellID = x.WellID.Value
                }).ToList();
        }

        public static WellDto CreateNew(ZybachDbContext dbContext, WellNewDto wellNewDto)
        {
            var well = new Well
            {
                CreateDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow,
                WellRegistrationID = wellNewDto.WellRegistrationID,
                WellGeometry = CreateWellGeometryFromLatLong(wellNewDto.Latitude, wellNewDto.Longitude)
            };
            well.StreamflowZoneID = dbContext.StreamFlowZones
                .FirstOrDefault(x => x.StreamFlowZoneGeometry.Intersects(well.WellGeometry))?.StreamFlowZoneID;
            dbContext.Wells.Add(well);
            dbContext.SaveChanges();
            dbContext.Entry(well).Reload();
            return GetByIDAsDto(dbContext, well.WellID);
        }

        private static Geometry CreateWellGeometryFromLatLong(double latitude, double longitude)
        {
            var point = new NetTopologySuite.Geometries.Point(longitude, latitude)
            {
                SRID = 4326
            };
            return point;
        }

        public static void MapFromContactUpsert(Well well, WellContactInfoDto wellContactInfoDto)
        {
            well.TownshipRangeSection = wellContactInfoDto.TownshipRangeSection;
            well.CountyID = wellContactInfoDto.CountyID;

            well.OwnerName = wellContactInfoDto.OwnerName;
            well.OwnerAddress = wellContactInfoDto.OwnerAddress;
            well.OwnerCity = wellContactInfoDto.OwnerCity;
            well.OwnerState = wellContactInfoDto.OwnerState;
            well.OwnerZipCode = wellContactInfoDto.OwnerZipCode;

            well.AdditionalContactName = wellContactInfoDto.AdditionalContactName;
            well.AdditionalContactAddress = wellContactInfoDto.AdditionalContactAddress;
            well.AdditionalContactCity = wellContactInfoDto.AdditionalContactCity;
            well.AdditionalContactState = wellContactInfoDto.AdditionalContactState;
            well.AdditionalContactZipCode = wellContactInfoDto.AdditionalContactZipCode;

            well.WellNickname = wellContactInfoDto.WellNickname;
            well.Notes = wellContactInfoDto.Notes;
        }

        public static void MapFromParticipationUpsert(Well well, WellParticipationInfoDto wellParticipationInfoDto)
        {
            well.WellParticipationID = wellParticipationInfoDto.WellParticipationID;
            well.WellUseID = wellParticipationInfoDto.WellUseID;
            well.RequiresChemigation = wellParticipationInfoDto.RequiresChemigation;
            well.RequiresWaterLevelInspection = wellParticipationInfoDto.RequiresWaterLevelInspection;
            well.IsReplacement = wellParticipationInfoDto.IsReplacement;
            well.WellDepth = wellParticipationInfoDto.WellDepth;
            well.Clearinghouse = wellParticipationInfoDto.Clearinghouse;
            well.PageNumber = wellParticipationInfoDto.PageNumber;
            well.ScreenDepth = wellParticipationInfoDto.ScreenDepth;
            well.ScreenInterval = wellParticipationInfoDto.ScreenInterval;
            well.SiteName = wellParticipationInfoDto.SiteName;
            well.SiteNumber = wellParticipationInfoDto.SiteNumber;
        }

        public static List<WellWaterQualityInspectionSummaryDto> ListAsWellWaterQualityInspectionDtos(ZybachDbContext dbContext)
        {
            return dbContext.Wells.AsNoTracking()
                .Include(x => x.WaterQualityInspections)
                .Include(x => x.WellParticipation)
                .Where(x => x.WaterQualityInspections.Any())
                .Select(x => x.AsWellWaterQualityInspectionSummaryDto()).ToList();
        }

        public static IEnumerable<WellWaterQualityInspectionDetailedDto> ListByWellIDsAsWellWaterQualityInspectionDetailedDto(ZybachDbContext dbContext, List<int> wellIDs)
        {
            var wells = dbContext.Wells
                .Include(x => x.WellWaterQualityInspectionTypes)
                .Include(x => x.WellParticipation)
                .AsNoTracking()
                .Where(x => wellIDs.Contains(x.WellID))
                .ToList();
            var waterQualityInspections = dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).Select(x => x.AsSimpleDto()).ToList());

            var wellWaterQualityInspectionDetailedDtos = wells
                .Select(x => x.AsWellWaterQualityInspectionDetailedDto(waterQualityInspections.ContainsKey(x.WellID) ? waterQualityInspections[x.WellID] : null))
                .ToList();
            return wellWaterQualityInspectionDetailedDtos;
        }

        public static List<WellWaterLevelMapSummaryDto> ListAsWaterLevelMapSummaryDtos(ZybachDbContext dbContext)
        {
            return GetWellsImpl(dbContext)
                .OrderBy(x => x.WellRegistrationID)
                .Select(x => x.AsWaterLevelMapSummaryDto())
                .ToList();
        }

        public static List<WellForFlowTestReportPageDto> ListForFlowTestReportPageDtos(ZybachDbContext dbContext)
        {
            var results = new List<WellForFlowTestReportPageDto>();
            
            #region Requires Flow Rate Test

            // MK 6/7/2024 -- Wells requiring pump rate auditing. From the card -- "Audited Pump Rate is 3 years old or greater (and due for a new flow test)".
            var threeYearsInThePast = DateTime.UtcNow.AddYears(-3); 
            var agHubWellsRequiringPumpRateAudit = dbContext.AgHubWells
                .Include(x => x.Well)
                .AsNoTracking()
                .Where(x => x.Well.RequiresWaterLevelInspection && (!x.AuditPumpRateTested.HasValue || x.AuditPumpRateTested.Value <= threeYearsInThePast))
                .Select(x => new WellForFlowTestReportPageDto()
                {
                    WellID = x.WellID,
                    WellRegistrationID = x.Well.WellRegistrationID,
                    FieldName = x.FieldName,
                    LastFlowTest = x.AuditPumpRateTested,
                    AgHubRegisteredUser = x.AgHubRegisteredUser,
                });

            results.AddRange(agHubWellsRequiringPumpRateAudit);

            #endregion

            #region Requires Chemigation Inspection

            // MK 6/7/2024 -- Wells that have inspections that are due for renewal. From the card -- "a chemigation inspection is due (pending for current year) which is determined based on every 3 years logic".
            var currentYear = DateTime.UtcNow.Year;
            var inspectionsRequiringRenewal = dbContext.ChemigationInspections
                .Include(x => x.ChemigationPermitAnnualRecord).ThenInclude(x => x.ChemigationPermit).ThenInclude(x => x.Well)
                .Include(x => x.ChemigationPermitAnnualRecord).ThenInclude(x => x.ChemigationPermitAnnualRecordApplicators)
                .AsNoTracking()
                .Where(x => x.ChemigationPermitAnnualRecord.ChemigationPermit.Well.RequiresChemigation && x.ChemigationPermitAnnualRecord.RecordYear == currentYear && x.ChemigationInspectionStatusID == ChemigationInspectionStatus.Pending.ChemigationInspectionStatusID);

            var agWells = dbContext.AgHubWells.AsNoTracking().ToList();
            foreach (var chemigationInspection in inspectionsRequiringRenewal)
            {
                var existingResult = results.SingleOrDefault(x => x.WellID == chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.WellID);
                if (existingResult != null)
                {
                    //MK 6/7/2024 -- This code block handles the case where a well is due for both a flow test and a chemigation inspection, not sure how often that happens, but it seems possible so lets handle it!
                    existingResult.ChemigationPermitNumber = chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumber;
                    existingResult.ChemigationPermitNumberDisplay = chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumberDisplay;
                    existingResult.ChemigationPermitApplicantFirstName = chemigationInspection.ChemigationPermitAnnualRecord.ApplicantFirstName;
                    existingResult.ChemigationPermitApplicantLastName = chemigationInspection.ChemigationPermitAnnualRecord.ApplicantLastName;
                    existingResult.ChemigationPermitApplicatorNames = string.Join(", ", chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermitAnnualRecordApplicators.Select(x => $"({x.ApplicatorName})"));
                }
                else if (chemigationInspection?.ChemigationPermitAnnualRecord?.ChemigationPermit?.WellID != null)
                {
                    //MK 6/7/2024 -- This code block handles the case where a well is due for a chemigation inspection, but wasn't due for a flow test.
                    //We still want to include some AgHubWell data (namely FieldName and AgHubRegisteredUserName) to help the users track down the place and person to contact for the inspection.
                    var agHubWell = agWells.FirstOrDefault(x => x.WellID == chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.WellID);
                    results.Add(new WellForFlowTestReportPageDto()
                    {
                        WellID = chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.WellID.Value,
                        WellRegistrationID = chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.Well.WellRegistrationID,
                        FieldName = agHubWell?.FieldName,
                        LastFlowTest = agHubWell?.AuditPumpRateTested,
                        AgHubRegisteredUser = agHubWell?.AgHubRegisteredUser,
                        ChemigationPermitNumber = chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumber,
                        ChemigationPermitNumberDisplay = chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumberDisplay,
                        ChemigationPermitApplicantFirstName = chemigationInspection.ChemigationPermitAnnualRecord.ApplicantFirstName,
                        ChemigationPermitApplicantLastName = chemigationInspection.ChemigationPermitAnnualRecord.ApplicantLastName,
                        ChemigationPermitApplicatorNames = string.Join(", ", chemigationInspection.ChemigationPermitAnnualRecord.ChemigationPermitAnnualRecordApplicators.Select(x =>$"({x.ApplicatorName})"))
                    });
                }
            }

            #endregion

            
            #region Get Last Inspection Date

            var chemigationInspections = dbContext.ChemigationInspections
                .Include(x => x.ChemigationPermitAnnualRecord).ThenInclude(chemigationPermitAnnualRecord => chemigationPermitAnnualRecord.ChemigationPermit)
                .AsNoTracking()
                .ToList();

            foreach (var result in results)
            {
                var lastChemigationInspection = chemigationInspections.Where(x => x.ChemigationPermitAnnualRecord.ChemigationPermit.WellID == result.WellID && x.InspectionDate.HasValue).MaxBy(x => x.InspectionDate);
                result.LastInspected = lastChemigationInspection?.InspectionDate;
            }

            #endregion

            return results;
        }
    }
}