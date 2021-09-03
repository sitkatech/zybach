import { ExecOptionsWithStringEncoding } from "child_process";

export class InstallationDto {
    Status: string;
    Date: string;
    Lon: number
    Lat: number;
    FlowmeterSerialNumber: string;
    SensorSerialNumber: string;
    InstallationCanonicalName: string;
    Photos: string[];
  PhotoDataUrls: any[];
  NoPhotoAvailable: boolean;
}
