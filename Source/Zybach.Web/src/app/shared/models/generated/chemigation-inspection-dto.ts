//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspection]


export class ChemigationInspectionDto {
	ChemigationInspectionID : number
	WellRegistrationID : string
	ProtocolCanonicalName : string
	Status : string
	LastUpdate : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
