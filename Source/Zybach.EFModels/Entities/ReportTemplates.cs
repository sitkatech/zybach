using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Zybach.Models.DataTransferObjects;
using Point = GeoJSON.Net.Geometry.Point;

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


        private static IQueryable<ReportTemplate> GetReportTemplatesImpl(ZybachDbContext dbContext)
        {
            return dbContext.ReportTemplates
                .Include(x => x.ReportTemplateModel)
                .Include(x => x.ReportTemplateModelType)
                .Include(x => x.FileResource)
                .Include(x => x.FileResource).ThenInclude(x => x.FileResourceMimeType)
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser)
                .Include(x => x.FileResource).ThenInclude(x => x.CreateUser).ThenInclude(x => x.Role)
                .AsNoTracking();
        }
    }
}