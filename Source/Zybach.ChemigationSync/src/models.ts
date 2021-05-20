import { Feature } from "geojson";

interface InspectionManifest {
    lastChangedDate: Date;
    insertMissingWells: boolean;        
    deleteOrphanedSamples: boolean;
    fieldAssignments: FieldAssignment[];
}

interface FieldAssignment {
    cname: string;
    name: string;
    startDate: Date;
    endDate: Date;

    protocol: Protocol,
    sampleNameTemplate: string,
    sites: Site[]
}

interface Protocol {
    cname: string,
    version: number
}

interface Site {
    cname: string,
    tags?: string[],
    properties?: { [key: string]: [string] },
    latitude: number,
    longitude: number
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