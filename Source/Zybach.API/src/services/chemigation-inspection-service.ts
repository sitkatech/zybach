import { ChemigationInspectionDto, ChemigationInspectionDtoFactory } from "../dtos/chemigation-inspection-dto";
import { InternalServerError } from "../errors/internal-server-error";
import ChemigationInspection, { ChemigationInspectionInterface } from "../models/chemigation-inspection";
import { provideSingleton } from "../util/provide-singleton";

@provideSingleton(ChemigationInspectionService)
export class ChemigationInspectionService {
    public async getChemigationInspections(): Promise<ChemigationInspectionDto[]> {
        try {
            const inspections = await ChemigationInspection.find();

            return inspections.map((x: ChemigationInspectionInterface)=>ChemigationInspectionDtoFactory.fromModel(x));
        } catch (error) {
            console.error(error);
            throw new InternalServerError(error.message);
        }
    }
}