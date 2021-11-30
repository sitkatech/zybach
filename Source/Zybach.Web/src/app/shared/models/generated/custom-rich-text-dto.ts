//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[CustomRichText]
import { CustomRichTextTypeDto } from './custom-rich-text-type-dto'

export class CustomRichTextDto {
	CustomRichTextID : number
	CustomRichTextType : CustomRichTextTypeDto
	CustomRichTextContent : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
