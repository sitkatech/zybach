import { NotFoundError } from "../errors/not-found-error";
import { provideSingleton } from "../util/provide-singleton";
import { CustomRichTextDto, CustomRichTextDtoFactory, CustomRichTextUpdateDto } from "../dtos/custom-rich-text-dto";
import CustomRichText from "../models/custom-rich-text";

@provideSingleton(CustomRichTextService)
export class CustomRichTextService {
    public async updateCustomRichText(customRichTextID: number, customRichTextUpdateDto: CustomRichTextUpdateDto): Promise<CustomRichTextDto> {
        const updatedCustomRichText = await CustomRichText.findOneAndUpdate({CustomRichTextID: customRichTextID}, customRichTextUpdateDto, {new: true});

        if (!updatedCustomRichText){
            throw new NotFoundError("Custom Rich Text not found");
        }

        return CustomRichTextDtoFactory.FromModel(updatedCustomRichText);
    }
    public async getByCustomRichTextID(customRichTextID: number) : Promise<CustomRichTextDto> {
        const customRichText = await CustomRichText.findOne({CustomRichTextID: customRichTextID});

        if (!customRichText){
            throw new NotFoundError("Custom Rich Text not found");
        }

        return CustomRichTextDtoFactory.FromModel(customRichText);
    }
}