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
                    .ThenInclude(x => x.ChemigationPermit)
                        .ThenInclude(x => x.Well)
                .Include(x => x.ChemigationInspectionFailureReason)
                .Include(x => x.ChemigationMainlineCheckValve)
                .Include(x => x.ChemigationInjectionValve)
                .Include(x => x.Tillage)
                .Include(x => x.CropType)
                .Include(x => x.InspectorUser)
                .AsNoTracking();
        }

        public static List<ChemigationInspectionSimpleDto> ListAsDto(ZybachDbContext dbContext)
        {
            return GetChemigationInspectionsImpl(dbContext).OrderByDescending(x => x.InspectionDate).Select(x => x.AsSimpleDto()).ToList();
        }

        public static ChemigationInspectionSimpleDto GetLatestChemigationInspectionByPermitNumber(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            return GetChemigationInspectionsImpl(dbContext)
                .Where(x => x.ChemigationPermitAnnualRecord.ChemigationPermit.ChemigationPermitNumber == chemigationPermitNumber)
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

        public static void CreateDefaultNewChemigationInspection(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID)
        {
            var blankChemigationInspectionUpsertDto = new ChemigationInspectionUpsertDto()
            {
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecordID,
                ChemigationInspectionStatusID = (int)ChemigationInspectionStatusEnum.Pending,
                ChemigationInspectionTypeID = (int)ChemigationInspectionTypeEnum.NEWINITIALORREACTIVATION,
                HasVacuumReliefValve = true,
                HasInspectionPort = true
            };

            CreateChemigationInspection(dbContext, blankChemigationInspectionUpsertDto);
        }
    }
}