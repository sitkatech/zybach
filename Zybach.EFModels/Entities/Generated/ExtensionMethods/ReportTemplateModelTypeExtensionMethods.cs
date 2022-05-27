//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplateModelType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ReportTemplateModelTypeExtensionMethods
    {
        public static ReportTemplateModelTypeDto AsDto(this ReportTemplateModelType reportTemplateModelType)
        {
            var reportTemplateModelTypeDto = new ReportTemplateModelTypeDto()
            {
                ReportTemplateModelTypeID = reportTemplateModelType.ReportTemplateModelTypeID,
                ReportTemplateModelTypeName = reportTemplateModelType.ReportTemplateModelTypeName,
                ReportTemplateModelTypeDisplayName = reportTemplateModelType.ReportTemplateModelTypeDisplayName,
                ReportTemplateModelTypeDescription = reportTemplateModelType.ReportTemplateModelTypeDescription
            };
            DoCustomMappings(reportTemplateModelType, reportTemplateModelTypeDto);
            return reportTemplateModelTypeDto;
        }

        static partial void DoCustomMappings(ReportTemplateModelType reportTemplateModelType, ReportTemplateModelTypeDto reportTemplateModelTypeDto);

        public static ReportTemplateModelTypeSimpleDto AsSimpleDto(this ReportTemplateModelType reportTemplateModelType)
        {
            var reportTemplateModelTypeSimpleDto = new ReportTemplateModelTypeSimpleDto()
            {
                ReportTemplateModelTypeID = reportTemplateModelType.ReportTemplateModelTypeID,
                ReportTemplateModelTypeName = reportTemplateModelType.ReportTemplateModelTypeName,
                ReportTemplateModelTypeDisplayName = reportTemplateModelType.ReportTemplateModelTypeDisplayName,
                ReportTemplateModelTypeDescription = reportTemplateModelType.ReportTemplateModelTypeDescription
            };
            DoCustomSimpleDtoMappings(reportTemplateModelType, reportTemplateModelTypeSimpleDto);
            return reportTemplateModelTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ReportTemplateModelType reportTemplateModelType, ReportTemplateModelTypeSimpleDto reportTemplateModelTypeSimpleDto);
    }
}