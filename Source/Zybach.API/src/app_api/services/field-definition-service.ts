import { NotFoundError } from "../../errors/not-found-error";
import { provideSingleton } from "../../util/provide-singleton";
import { FieldDefinitionDto, FieldDefinitionDtoFactory, FieldDefinitionUpdateDto } from "../dtos/field-definition-dto";
import FieldDefinition, { FieldDefinitionInterface } from "../models/field-definition";

@provideSingleton(FieldDefinitionService)
export class FieldDefinitionService {
    public async update(fieldDefinitionID: number, fieldDefinitionUpdateDto: FieldDefinitionUpdateDto): Promise<FieldDefinitionDto> {
        let updatedFieldDefinition = await FieldDefinition.findOneAndUpdate({FieldDefinitionID: fieldDefinitionID}, fieldDefinitionUpdateDto, {new: true});
        
        if (!updatedFieldDefinition){
            throw new NotFoundError("Field Definition not found");
        }
        
        return FieldDefinitionDtoFactory.FromModel(updatedFieldDefinition);
    }

    public async getAll(): Promise<FieldDefinitionDto[]> {
        const fieldDefinitions = await FieldDefinition.find();

        const fieldDefinitionDtos = fieldDefinitions
            .map((x: FieldDefinitionInterface) => FieldDefinitionDtoFactory.FromModel(x));

        return fieldDefinitionDtos;
    }

    public async getByFieldDefinitionID(fieldDefinitionID: number) : Promise<FieldDefinitionDto> {
        const fieldDefinition =  await FieldDefinition.findOne({FieldDefinitionID: fieldDefinitionID})
        
        if (!fieldDefinition) {
            throw new NotFoundError("Field Definition not found");
        }

        return FieldDefinitionDtoFactory.FromModel(fieldDefinition);
    }
}
