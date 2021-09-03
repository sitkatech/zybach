using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationInspection
    {
        public static List<ChemigationInspectionDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspections.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}