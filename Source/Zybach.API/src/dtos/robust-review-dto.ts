export interface RobustReviewDto {
    wellRegistrationID: string;
    wellTPID?: string;
    lat: number;
    long: number;
    dataSource: string;
    monthlyPumpedVolumeGallons: MonthlyPumpedVolumeGallonsDto[];
}

export interface MonthlyPumpedVolumeGallonsDto {
    month: number;
    year: number;
    volumePumpedGallons: number;
}
