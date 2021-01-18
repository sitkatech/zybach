import CustomRichText, { CustomRichTextInterface } from "../models/custom-rich-text";

export class CustomRichTextDto {
	CustomRichTextID? : number
	CustomRichTextType? : CustomRichTextTypeDto
	CustomRichTextContent? : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

export class CustomRichTextTypeDto {
	CustomRichTextTypeID? : number
	CustomRichTextTypeName? : string
	CustomRichTextTypeDisplayName? : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

export interface CustomRichTextUpdateDto {
    CustomRichTextContent: string
}

export class CustomRichTextDtoFactory {
    public static FromModel (model: CustomRichTextInterface): CustomRichTextDto{
        const customRichTextType = {
            CustomRichTextTypeID: model.CustomRichTextID,
            CustomRichTextTypeName: model.CustomRichTextName,
            CustomRichTextTypeDisplayName: model.CustomRichTextDisplayName
        }

        return new CustomRichTextDto({
            CustomRichTextID: model.CustomRichTextID,
            CustomRichTextType: customRichTextType,
            CustomRichTextContent: model.CustomRichTextContent
        });
    }
}