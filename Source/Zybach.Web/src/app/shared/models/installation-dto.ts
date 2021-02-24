import { ExecOptionsWithStringEncoding } from "child_process";

export class InstallationDto {
    status: string;
    date: string;
    lon: number
    lat: number;
    flowmeterSerialNumber: string;
    sensorSerialNumber: string;
}
