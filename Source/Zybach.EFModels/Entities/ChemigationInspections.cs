using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInspections
    {
        public static IQueryable<ChemigationInspection> GetChemigationInspectionsImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspections
                .Include(x => x.ChemigationPermitAnnualRecord)
                    .ThenInclude(x => x.ChemigationPermitAnnualRecordStatus)
                .Include(x => x.ChemigationPermitAnnualRecord)
                    .ThenInclude(x => x.ChemigationInjectionUnitType)
                .Include(x => x.ChemigationPermitAnnualRecord)
                    .ThenInclude(x => x.ChemigationPermit)
                        .ThenInclude(x => x.ChemigationPermitStatus)
                .Include(x => x.ChemigationPermitAnnualRecord)
                    .ThenInclude(x => x.ChemigationPermit)
                        .ThenInclude(x => x.County)
                .Include(x => x.ChemigationPermitAnnualRecord)
                    .ThenInclude(x => x.ChemigationPermit)
                        .ThenInclude(x => x.Well)
                .Include(x => x.ChemigationInspectionStatus)
                .Include(x => x.ChemigationInspectionFailureReason)
                .Include(x => x.ChemigationInspectionType)
                .Include(x => x.ChemigationMainlineCheckValve)
                .Include(x => x.ChemigationLowPressureValve)
                .Include(x => x.ChemigationInjectionValve)
                .Include(x => x.ChemigationInterlockType)
                .Include(x => x.Tillage)
                .Include(x => x.CropType)
                .Include(x => x.InspectorUser)
                    .ThenInclude(x => x.Role)
                .AsNoTracking();
        }

        public static List<ChemigationInspectionSimpleDto> ListAsDto(ZybachDbContext dbContext)
        {
            return GetChemigationInspectionsImpl(dbContext).OrderByDescending(x => x.InspectionDate).Select(x => x.AsSimpleDto()).ToList();
        }

        public static ChemigationInspectionSimpleDto GetLatestChemigationInspectionByPermitNumber(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecordIDs = ChemigationPermitAnnualRecords
                .GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Where(x => x.ChemigationPermit.ChemigationPermitNumber == chemigationPermitNumber)
                .Select(x => x.ChemigationPermitAnnualRecordID).ToList();

            return GetChemigationInspectionsImpl(dbContext)
                .Where(x => chemigationPermitAnnualRecordIDs.Contains(x.ChemigationPermitAnnualRecordID))
                .OrderByDescending(y => y.InspectionDate)
                .FirstOrDefault()?.AsSimpleDto();
        }

        public static ChemigationInspectionSimpleDto GetChemigationInspectionSimpleDtoByID(ZybachDbContext dbContext, int chemigationInspectionID)
        {
            return GetChemigationInspectionsImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationInspectionID == chemigationInspectionID)?.AsSimpleDto();
        }

        public static ChemigationInspectionSimpleDto CreateChemigationInspection(ZybachDbContext dbContext, ChemigationInspectionUpsertDto chemigationInspectionUpsertDto)
        {
            if (chemigationInspectionUpsertDto == null)
            {
                return null;
            }

            var chemigationInspection = new ChemigationInspection()
            {
                ChemigationPermitAnnualRecordID = chemigationInspectionUpsertDto.ChemigationPermitAnnualRecordID,
                ChemigationInspectionStatusID = chemigationInspectionUpsertDto.ChemigationInspectionStatusID,
                ChemigationInspectionFailureReasonID = chemigationInspectionUpsertDto.ChemigationInspectionFailureReasonID,
                ChemigationInspectionTypeID = chemigationInspectionUpsertDto.ChemigationInspectionTypeID,
                InspectionDate = chemigationInspectionUpsertDto.InspectionDate?.AddHours(8),
                InspectorUserID = chemigationInspectionUpsertDto.InspectorUserID,
                ChemigationMainlineCheckValveID = chemigationInspectionUpsertDto.ChemigationMainlineCheckValveID,
                ChemigationLowPressureValveID = chemigationInspectionUpsertDto.ChemigationLowPressureValveID,
                ChemigationInjectionValveID = chemigationInspectionUpsertDto.ChemigationInjectionValveID,
                ChemigationInterlockTypeID = chemigationInspectionUpsertDto.ChemigationInterlockTypeID,
                HasVacuumReliefValve = chemigationInspectionUpsertDto.HasVacuumReliefValve,
                HasInspectionPort = chemigationInspectionUpsertDto.HasInspectionPort,
                TillageID = chemigationInspectionUpsertDto.TillageID,
                CropTypeID = chemigationInspectionUpsertDto.CropTypeID,
                InspectionNotes = chemigationInspectionUpsertDto.InspectionNotes
            };

            dbContext.ChemigationInspections.Add(chemigationInspection);
            dbContext.SaveChanges();
            dbContext.Entry(chemigationInspection).Reload();

            return GetChemigationInspectionSimpleDtoByID(dbContext, chemigationInspection.ChemigationInspectionID);
        }

        public static void CreateBlankChemigationInspection(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID)
        {
            var blankChemigationInspectionUpsertDto = new ChemigationInspectionUpsertDto()
            {
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecordID,
                ChemigationInspectionStatusID = (int)ChemigationInspectionStatuses.ChemigationInspectionStatusEnum.Pending,
                ChemigationInspectionFailureReasonID = null,
                ChemigationInspectionTypeID = (int)ChemigationInspectionTypes.ChemigationInspectionTypeEnum.NewInitialOrReactivation,
                InspectionDate = null,
                InspectorUserID = null,
                ChemigationMainlineCheckValveID = null,
                ChemigationLowPressureValveID = null,
                ChemigationInjectionValveID = null,
                ChemigationInterlockTypeID = null,
                HasVacuumReliefValve = true,
                HasInspectionPort = true,
                TillageID = null,
                CropTypeID = null,
                InspectionNotes = null
            };

            CreateChemigationInspection(dbContext, blankChemigationInspectionUpsertDto);
        }
    }
}