//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplate]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ReportTemplateDto
    {
        public int ReportTemplateID { get; set; }
        public FileResourceDto FileResource { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ReportTemplateModelTypeDto ReportTemplateModelType { get; set; }
        public ReportTemplateModelDto ReportTemplateModel { get; set; }
    }

    public partial class ReportTemplateSimpleDto
    {
        public int ReportTemplateID { get; set; }
        public System.Int32 FileResourceID { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public System.Int32 ReportTemplateModelTypeID { get; set; }
        public System.Int32 ReportTemplateModelID { get; set; }
    }

}