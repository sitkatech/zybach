import { Controller, Route, Security, Get, Hidden } from "tsoa";
import { FieldDefinitionDto } from "../dtos/field-definition-dto";
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
}
