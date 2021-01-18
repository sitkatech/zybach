import { Query } from "mongoose";
import { Body, Controller, Post, Route, Request, Security, Get, Path, Hidden, Put } from "tsoa";
import { ApiError } from "../../errors/apiError";
import { CustomRichTextDto, CustomRichTextUpdateDto } from "../dtos/custom-rich-text-dto";
import { UserCreateDto } from "../dtos/user-create-dto";
import { UserDto } from "../dtos/user-dto";
import CustomRichText from "../models/custom-rich-text";
import { RoleEnum } from "../models/role";
import User from "../models/user";
import { RequestWithUserContext } from "../request-with-user-context";
import { SecurityType } from "../security/authentication";
import { CustomRichTextService } from "../services/custom-rich-text-service";
import { UserService } from "../services/user-service";


@Route("/api/customRichText")
@Hidden()
export class CustomRichTextController extends Controller{
    @Get("{customRichTextID}")
    @Security(SecurityType.ANONYMOUS)
    public async getCustomRichText(
        @Path() customRichTextID: number
    ): Promise<CustomRichTextDto> {
        return await new CustomRichTextService().getByCustomRichTextID(customRichTextID);
    }

    @Put("{customRichTextID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async updateCustomRichText(
        @Path() customRichTextID: number,
        @Body() customRichTextUpdateDto: CustomRichTextUpdateDto
    ) : Promise<CustomRichTextDto> {
        return await new CustomRichTextService().updateCustomRichText(customRichTextID, customRichTextUpdateDto);
    }
}
