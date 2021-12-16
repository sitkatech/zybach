using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class ReportTemplates
    {
        public static List<ReportTemplateDto> ListAsDtos(ZybachDbContext dbContext)
        {
            return GetReportTemplatesImpl(dbContext, false).Select(x => x.AsDto()).ToList();
        }

        public static ReportTemplateDto GetByReportTemplateIDAsDto(ZybachDbContext dbContext, int reportTemplateID)
        {
            var reportTemplate = GetReportTemplatesImpl(dbContext, false).SingleOrDefault(x => x.ReportTemplateID == reportTemplateID);
            return reportTemplate?.AsDto();
        }

        public static ReportTemplate GetByReportTemplateID(ZybachDbContext dbContext, int reportTemplateID)
        {
            return GetByReportTemplateID(dbContext, reportTemplateID, false); 
        }

        public static ReportTemplate GetByReportTemplateID(ZybachDbContext dbContext, int reportTemplateID, bool forUpdate)
        {
            var reportTemplate = GetReportTemplatesImpl(dbContext, forUpdate)
                .OrderBy(x => x.ReportTemplateID)
                .FirstOrDefault(x => x.ReportTemplateID == reportTemplateID);
            return reportTemplate;
        }

        private static IQueryable<ReportTemplate> GetReportTemplatesImpl(ZybachDbContext dbContext, bool forUpdate)
        {
            var reportTemplates = dbContext.ReportTemplates
                .Include(x => x.ReportTemplateModel)
                .Include(x => x.ReportTemplateModelType)
                .Include(x => x.FileResource)
                .Include(x => x.FileResource).ThenInclude(x => x.FileResourceMimeType)
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser)
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser).ThenInclude(x => x.Role);
            if (!forUpdate)
            {
                reportTemplates.AsNoTracking();
            }
            return reportTemplates;
        }
    }
}