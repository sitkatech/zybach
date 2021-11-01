import { FileResourceDto } from "./generated/file-resource-dto"
import { ReportTemplateModelDto } from "./generated/report-template-model-dto"
import { ReportTemplateModelTypeDto } from "./generated/report-template-model-type-dto"

export class ReportTemplateNewDto {
	ReportTemplateID : number
	DisplayName : string
	Description : string
	ReportTemplateModelID : number
    FileResource: File
}