using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class ReportTemplateModel
    {
        public static IEnumerable<ReportTemplateModelDto> List(ZybachDbContext dbContext)
        {
            var reportTemplateModels = dbContext.ReportTemplateModels
                .AsNoTracking()
                .Select(x => x.AsDto());

            return reportTemplateModels;
        }
    }

    public enum ReportTemplateModelEnum
    {
        ChemigationPermit = 1
    }
}