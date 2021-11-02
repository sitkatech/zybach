using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class ReportTemplateModelType
    {
    }

    public enum ReportTemplateModelTypeEnum
    {
        SingleModel = 1,
        MultipleModels = 2
    }
}