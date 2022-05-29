using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ReportTemplateModel
    {
        public static IEnumerable<ReportTemplateModelDto> List(ZybachDbContext dbContext)
        {
            return ReportTemplateModel.AllAsDto;
        }
    }
}