//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplate]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ReportTemplateExtensionMethods
    {
        public static ReportTemplateDto AsDto(this ReportTemplate reportTemplate)
        {
            var reportTemplateDto = new ReportTemplateDto()
            {
                ReportTemplateID = reportTemplate.ReportTemplateID,
                FileResource = reportTemplate.FileResource.AsDto(),
                DisplayName = reportTemplate.DisplayName,
                Description = reportTemplate.Description,
                ReportTemplateModelType = reportTemplate.ReportTemplateModelType.AsDto(),
                ReportTemplateModel = reportTemplate.ReportTemplateModel.AsDto()
            };
            DoCustomMappings(reportTemplate, reportTemplateDto);
            return reportTemplateDto;
        }

        static partial void DoCustomMappings(ReportTemplate reportTemplate, ReportTemplateDto reportTemplateDto);

    }
}