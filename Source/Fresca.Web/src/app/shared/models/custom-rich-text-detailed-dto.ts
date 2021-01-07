import { CustomRichTextDto } from "./generated/custom-rich-text-dto"
export class CustomRichTextDetailedDto extends CustomRichTextDto{
    public IsEmptyContent: boolean;

    constructor(obj?: any){
        super();
        Object.assign(this, obj);
    }
}