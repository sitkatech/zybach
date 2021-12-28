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
                .Include(x => x.Tillage)
                .Include(x => x.CropType)
                .Include(x => x.InspectorUser)
                    .ThenInclude(x => x.Role)
                .AsNoTracking();
        }

        public static List<ChemigationInspectionSimpleDto> ListAsDto(ZybachDbContext dbContext)
        {
            return GetChemigationInspectionsImpl(dbContext).Select(x => x.AsSimpleDto()).ToList();
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

        public static ChemigationInspectionSimpleDto UpdateChemigationInspectionByID(ZybachDbContext dbContext, int chemigationInspectionID, ChemigationInspectionUpsertDto chemigationInspectionUpsertDto)
        {
            var chemigationInspection = dbContext.ChemigationInspections.SingleOrDefault(x => x.ChemigationInspectionID == chemigationInspectionID);

            if (chemigationInspection == null || chemigationInspectionUpsertDto == null)
            {
                return null;
            }

            chemigationInspection.ChemigationPermitAnnualRecordID =
                chemigationInspectionUpsertDto.ChemigationPermitAnnualRecordID;
            chemigationInspection.ChemigationInspectionStatusID =
                chemigationInspectionUpsertDto.ChemigationInspectionStatusID;
            chemigationInspection.ChemigationInspectionFailureReasonID =
                chemigationInspectionUpsertDto.ChemigationInspectionFailureReasonID;
            chemigationInspection.ChemigationInspectionTypeID =
                chemigationInspectionUpsertDto.ChemigationInspectionTypeID;
            chemigationInspection.InspectionDate = chemigationInspectionUpsertDto.InspectionDate;
            chemigationInspection.InspectorUserID = chemigationInspectionUpsertDto.InspectorUserID;
            chemigationInspection.ChemigationMainlineCheckValveID =
                chemigationInspectionUpsertDto.ChemigationMainlineCheckValveID;
            chemigationInspection.ChemigationLowPressureValveID =
                chemigationInspectionUpsertDto.ChemigationLowPressureValveID;
            chemigationInspection.ChemigationInjectionValveID =
                chemigationInspectionUpsertDto.ChemigationInjectionValveID;
            chemigationInspection.HasVacuumReliefValve = chemigationInspectionUpsertDto.HasVacuumReliefValve;
            chemigationInspection.HasInspectionPort = chemigationInspectionUpsertDto.HasInspectionPort;
            chemigationInspection.TillageID = chemigationInspectionUpsertDto.TillageID;
            chemigationInspection.CropTypeID = chemigationInspectionUpsertDto.CropTypeID;
            chemigationInspection.InspectionNotes = chemigationInspectionUpsertDto.InspectionNotes;
            
            dbContext.SaveChanges();

            return chemigationInspection.AsSimpleDto();
        }

        public static void DeleteByInspectionID(ZybachDbContext dbContext, int chemigationInspectionID)
        {
            var chemigationInspectionToRemove = dbContext.ChemigationInspections.SingleOrDefault(x => x.ChemigationInspectionID == chemigationInspectionID);

            if (chemigationInspectionToRemove != null)
            {
                dbContext.ChemigationInspections.Remove(chemigationInspectionToRemove);
                dbContext.SaveChanges();
            }
        }
    }
}