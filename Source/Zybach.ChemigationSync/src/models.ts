import { Feature } from "geojson";

interface InspectionManifest {
    LastChangedDate: Date;
    LastProcessedDate?: Date;
    InsertMissingWells: boolean;        
    DeleteOrphanedSamples: boolean;
    FieldAssignments: FieldAssignment[];
}

interface FieldAssignment {
    CanonicalName: string;
    Name: string;
    StartDate: Date;
    FinishDate: Date;

    Protocol: Protocol,
    SampleNameTemplate: string,
    Sites: Site[]
}

interface Protocol {
    CanonicalName: string,
    Version: number
}

interface Site {
    CanonicalName: string,
    Tags?: string[],
    Properties?: { [key: string]: [string] },
    Latitude: number,
    Longitude: number
}

interface WorkOrder {
    Name: string,
    CanonicalName: string,
    Description: string,
    StartDate: Date,
    FinishDate: Date,
    TeamMembers: []
}

interface GeoOptixSite {
    Name: string;
    CanonicalName: string;
    Tags?: string[];
    Location: Feature;
    Description: string;
    Properties?: { [key: string]: [string] };
}

interface Sample {
    SiteCanonicalName: string;
    Name: string;
    CanonicalName: string;
    ProtocolVersionNumber: number;
    WorkOrderCanonicalName: string;
    ProtocolCanonicalName: string;
    SampleDate: Date;
    MethodUpdateDate: Date;
    Tags: string[];
}


export {InspectionManifest, Protocol, FieldAssignment, Site, WorkOrder, GeoOptixSite, Sample}