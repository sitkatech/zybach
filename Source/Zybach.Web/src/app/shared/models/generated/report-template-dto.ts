//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplate]
import { FileResourceDto } from './file-resource-dto'
import { ReportTemplateModelTypeDto } from './report-template-model-type-dto'
import { ReportTemplateModelDto } from './report-template-model-dto'

export class ReportTemplateDto {
	ReportTemplateID : number
	FileResource : FileResourceDto
	DisplayName : string
	Description : string
	ReportTemplateModelType : ReportTemplateModelTypeDto
	ReportTemplateModel : ReportTemplateModelDto

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
