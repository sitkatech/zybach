export interface WellInspectionSummaryDto {
    wellRegistrationID: string;
    lastChemigationDate: Date;
    lastNitratesDate: Date;
    lastWaterLevelDate: Date;
    lastWaterQualitydate: Date;
    pendingInspectionsCount: number;
}