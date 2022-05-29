//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplate]
namespace Zybach.EFModels.Entities
{
    public partial class ReportTemplate
    {
        public ReportTemplateModelType ReportTemplateModelType => ReportTemplateModelType.AllLookupDictionary[ReportTemplateModelTypeID];
        public ReportTemplateModel ReportTemplateModel => ReportTemplateModel.AllLookupDictionary[ReportTemplateModelID];
    }
}