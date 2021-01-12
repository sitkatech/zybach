import { Controller, Route, Security, Get, Hidden, Path, Put, Body } from "tsoa";
import { FieldDefinitionDto, FieldDefinitionUpdateDto } from "../dtos/field-definition-dto";
import { SecurityType } from "../security/authentication";
import { FieldDefinitionService } from "../services/field-definition-service";


@Route("/api/fieldDefinitions")
@Hidden()
export class FieldDefinitionController extends Controller{
    @Get("")
    @Security(SecurityType.ANONYMOUS)
    public async list() : Promise<FieldDefinitionDto[]>{
        return await new FieldDefinitionService().getAll();
    }

    @Get("{fieldDefinitionID}")
    @Security(SecurityType.ANONYMOUS)
    public async getByFieldDefinitionID(
        @Path() fieldDefinitionID: number
    ) : Promise<FieldDefinitionDto> {
        return await new FieldDefinitionService().getByFieldDefinitionID(fieldDefinitionID);
    }
    
    @Put("{fieldDefinitionID}")
    @Security(SecurityType.KEYSTONE /*TODO scopes*/)
    public async update(
        @Path() fieldDefinitionID: number,
        @Body() fieldDefinitionUpdateDto: FieldDefinitionUpdateDto
    ) : Promise<FieldDefinitionDto> {
        return await new FieldDefinitionService().update(fieldDefinitionID, fieldDefinitionUpdateDto);
    }
}
