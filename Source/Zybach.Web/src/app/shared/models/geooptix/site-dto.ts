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