import { ChemigationInspectionInterface } from "../models/chemigation-inspection";

export interface ChemigationInspectionDto {
    wellRegistrationID: string
    protocolCanonicalName: string
    status: string
    lastUpdate: Date
}


export class ChemigationInspectionDtoFactory {
    public static fromModel(model: ChemigationInspectionInterface) : ChemigationInspectionDto{
        
        return {
            wellRegistrationID: model.wellRegistrationID,
            protocolCanonicalName: model.protocolCanonicalName,
            status: model.status,
            lastUpdate: model.lastUpdate,
        }
    }
}
