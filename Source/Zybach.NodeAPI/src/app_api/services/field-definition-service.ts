import { FieldDefinitionDto, FieldDefinitionDtoFactory } from "../dtos/field-definition-dto";
import FieldDefinition, { FieldDefinitionInterface } from "../models/field-definition";

export class FieldDefinitionService {
    public async getAll(): Promise<FieldDefinitionDto[]> {
        const fieldDefinitions = await FieldDefinition.find();

        const fieldDefinitionDtos = fieldDefinitions
            .map((x: FieldDefinitionInterface) => FieldDefinitionDtoFactory.FromModel(x));

        return fieldDefinitionDtos;
    }
}
