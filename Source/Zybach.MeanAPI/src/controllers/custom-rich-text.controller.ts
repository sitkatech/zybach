import { inject } from "inversify";
import { Body, Controller, Route, Security, Get, Path, Hidden, Put } from "tsoa";
import { provideSingleton } from "../util/provide-singleton";
import { CustomRichTextDto, CustomRichTextUpdateDto } from "../dtos/custom-rich-text-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { CustomRichTextService } from "../services/custom-rich-text-service";


@Route("/api/customRichText")
@Hidden()
@provideSingleton(CustomRichTextController)
export class CustomRichTextController extends Controller{
    constructor(@inject(CustomRichTextService) private customRichTextService: CustomRichTextService){
        super();
    }

    @Get("{customRichTextID}")
    @Security(SecurityType.ANONYMOUS)
    public async getCustomRichText(
        @Path() customRichTextID: number
    ): Promise<CustomRichTextDto> {
        return await this.customRichTextService.getByCustomRichTextID(customRichTextID);
    }

    @Put("{customRichTextID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async updateCustomRichText(
        @Path() customRichTextID: number,
        @Body() customRichTextUpdateDto: CustomRichTextUpdateDto
    ) : Promise<CustomRichTextDto> {
        return await this.customRichTextService.updateCustomRichText(customRichTextID, customRichTextUpdateDto);
    }
}
