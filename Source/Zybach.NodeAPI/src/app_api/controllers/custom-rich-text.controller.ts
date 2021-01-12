import { Query } from "mongoose";
import { Body, Controller, Post, Route, Request, Security, Get, Path, Hidden } from "tsoa";
import { ApiError } from "../../errors/apiError";
import { UserCreateDto } from "../dtos/user-create-dto";
import { UserDto } from "../dtos/user-dto";
import User from "../models/user";
import { RequestWithUserContext } from "../request-with-user-context";
import { SecurityType } from "../security/authentication";
import { UserService } from "../services/user-service";


@Route("/api/customRichText")
@Hidden()
export class CustomRichTextController extends Controller{
    @Get("{customRichTextID}")
    @Security(SecurityType.ANONYMOUS)
    public async getCustomRichText(
        @Path() customRichTextID: number
    ) {
        throw new ApiError("Internal Server Error", 500, "Not Implemented");
    }

}
