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
            return GetReportTemplatesImpl(dbContext).Select(x => x.AsDto()).ToList();
        }

        public static ReportTemplateDto GetByReportTemplateIDAsDto(ZybachDbContext dbContext, int reportTemplateID)
        {
            var reportTemplate = GetReportTemplatesImpl(dbContext).SingleOrDefault(x => x.ReportTemplateID == reportTemplateID);
            return reportTemplate?.AsDto();
        }

        public static ReportTemplate GetByReportTemplateID(ZybachDbContext dbContext, int reportTemplateID)
        {
            var reportTemplate = GetReportTemplatesImpl(dbContext)
                .SingleOrDefault(x => x.ReportTemplateID == reportTemplateID);
            return reportTemplate;
        }

        private static IQueryable<ReportTemplate> GetReportTemplatesImpl(ZybachDbContext dbContext)
        {
            return dbContext.ReportTemplates
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser)
                .AsNoTracking();
        }

        private static IQueryable<ReportTemplate> GetReportTemplatesImplWithTracking(ZybachDbContext dbContext)
        {
            return dbContext.ReportTemplates
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser);
        }

        public static List<ReportTemplateDto> ListByModelIDAsDtos(ZybachDbContext dbContext, int reportTemplateModelID)
        {
            var reportTemplates = GetReportTemplatesImpl(dbContext)
                .Where(x => x.ReportTemplateModelID == reportTemplateModelID)
                .Select(x => x.AsDto())
                .ToList();

            return reportTemplates;
        }

        public static ReportTemplate GetByReportTemplateIDWithTracking(ZybachDbContext dbContext, int reportTemplateID)
        {
            var reportTemplate = GetReportTemplatesImplWithTracking(dbContext)
                .SingleOrDefault(x => x.ReportTemplateID == reportTemplateID);
            return reportTemplate;
        }
    }
}