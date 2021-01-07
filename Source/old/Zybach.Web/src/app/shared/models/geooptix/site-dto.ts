import { Feature } from 'geojson';

export class SiteDto {
    ProjectCanonicalName: string;
    ProjectName: string;
    Protocols: any[];
    SampleCount: number;
    WorkOrderCount: number;
    SiteID: number;
    ProgramID: number;
    ProjectID: number;
    StudyDesignID: number;
    Name: string;
    CanonicalName: string;
    Description: string;
    Tags: string[]
    Location: Feature;
    CreateDate: Date;
    CreateUserID: number;
    UpdateDate: number;
    UpdateUserID: number;
}

export class StationDto {
    ProjectName: string;
    Site: SiteSummaryDto;
    HasFilesOrFolders: boolean;
    StationID: number;
    CreateDate: Date;
    CreateUserID: number;
    UpdateDate: Date;
    UpdateUserID: number;
    StationTypeID: number;
    ProjectCanonicalName: string;
    SiteCanonicalName: string;
    CanonicalName: string;
    Name: string;
    Description: string;
    Definition: StationDefinitionDto;
}

export class SiteSummaryDto {
    Name: string;
    CanonicalName: string;
    ProjectCanonicalName: string;
    Description: string;
    Location: Feature;
}

export class StationDefinitionDto{
    authToken: string;
    sensorType: string;
    dataDestinations: DataDestinationDto[];
}

export class DataDestinationDto{
    destinationType: string;
    formats: string[];
}

export class WellDto extends SiteDto {
    Sensor?: StationDto;
    LastReading?: Date;
}