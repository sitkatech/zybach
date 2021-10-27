//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplateModel]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ReportTemplateModelExtensionMethods
    {
        public static ReportTemplateModelDto AsDto(this ReportTemplateModel reportTemplateModel)
        {
            var reportTemplateModelDto = new ReportTemplateModelDto()
            {
                ReportTemplateModelID = reportTemplateModel.ReportTemplateModelID,
                ReportTemplateModelName = reportTemplateModel.ReportTemplateModelName,
                ReportTemplateModelDisplayName = reportTemplateModel.ReportTemplateModelDisplayName,
                ReportTemplateModelDescription = reportTemplateModel.ReportTemplateModelDescription
            };
            DoCustomMappings(reportTemplateModel, reportTemplateModelDto);
            return reportTemplateModelDto;
        }

        static partial void DoCustomMappings(ReportTemplateModel reportTemplateModel, ReportTemplateModelDto reportTemplateModelDto);

    }
}