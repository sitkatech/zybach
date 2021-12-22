using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationInspection
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

        public static List<ChemigationInspectionSimpleDto> List(ZybachDbContext dbContext)
        {
            return GetChemigationInspectionsImpl(dbContext).Select(x => x.AsSimpleDto()).ToList();
        }
    }
}