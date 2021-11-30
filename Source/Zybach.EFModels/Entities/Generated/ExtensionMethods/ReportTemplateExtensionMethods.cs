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

        public static ReportTemplateSimpleDto AsSimpleDto(this ReportTemplate reportTemplate)
        {
            var reportTemplateSimpleDto = new ReportTemplateSimpleDto()
            {
                ReportTemplateID = reportTemplate.ReportTemplateID,
                FileResourceID = reportTemplate.FileResourceID,
                DisplayName = reportTemplate.DisplayName,
                Description = reportTemplate.Description,
                ReportTemplateModelTypeID = reportTemplate.ReportTemplateModelTypeID,
                ReportTemplateModelID = reportTemplate.ReportTemplateModelID
            };
            DoCustomSimpleDtoMappings(reportTemplate, reportTemplateSimpleDto);
            return reportTemplateSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ReportTemplate reportTemplate, ReportTemplateSimpleDto reportTemplateSimpleDto);
    }
}