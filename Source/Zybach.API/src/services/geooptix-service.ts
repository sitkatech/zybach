import axios from "axios";
import { InternalServerError } from "../errors/internal-server-error";
import secrets from "../secrets";
import { provideSingleton } from "../util/provide-singleton";
import { SensorSummaryDto, SensorTypeMap, WellSummaryDto, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { WellController } from "../controllers/wells.controller";
import { GeoOptixObjectTypeToZybachObjectType, SearchSummaryDto } from "../dtos/search-summary-dto";

const FileReader = require('filereader');

@provideSingleton(GeoOptixService)
export class GeoOptixService {
    baseUrl: string;
    searchUrl: string;
    headers: { "x-geooptix-token": string };
    constructor(){
        this.baseUrl = secrets.GEOOPTIX_HOSTNAME;
        this.searchUrl = secrets.GEOOPTIX_SEARCH_HOSTNAME;
        this.headers = {
            "x-geooptix-token": secrets.GEOOPTIX_API_KEY
        }
    }

    public async getWellSummaries(): Promise<WellSummaryDto[]> {
        try {
            // todo: this getting stuff from GeoOptix needs to live in GeoOptixService class
            const geoOptixRequest = await axios.get(
                `${this.baseUrl}/project-overview-web/water-data-program/sites`,
                {
                    headers: this.headers
                }
            );

            return geoOptixRequest.data
                .map((x: { CanonicalName: any; Description: any; Location: any; }) =>
                    ({ wellRegistrationID: x.CanonicalName, description: x.Description, location: x.Location }));
        }
        catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getWellSummariesCreatedAsOfYear(year: number): Promise<WellSummaryDto[]> {
        try {
            // todo: this getting stuff from GeoOptix needs to live in GeoOptixService class
            const geoOptixRequest = await axios.get(
                `${this.baseUrl}/project-overview-web/water-data-program/sites`,
                {
                    headers: this.headers
                }
            );

            const filteredWells = geoOptixRequest.data.filter((x: any) => new Date(x.CreateDate).getFullYear() <= year);

            return filteredWells
                .map((x: { CanonicalName: any; Description: any; Location: any; }) =>
                    ({ wellRegistrationID: x.CanonicalName, description: x.Description, location: x.Location }));
        }
        catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getWellSummary(wellRegistrationID: string): Promise<WellSummaryDto | null> {
        try {
            const geoOptixRequest = await axios.get(
                `${this.baseUrl}/project-overview-web/water-data-program/sites/${wellRegistrationID}`,
                {
                    headers: this.headers,
                    validateStatus: (x: number) => true
                }
            );

            if (geoOptixRequest.status === 404 || geoOptixRequest.status === 204){
                return null;
            }

            return {
                location: geoOptixRequest.data.location,
                wellRegistrationID: wellRegistrationID
            };
        }
        catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getSensorSummaries(): Promise<SensorSummaryDto[]> {
        try {
            const geoOptixRequest = await axios.get(
                `${this.baseUrl}/project-overview-web/water-data-program/stations`,
                {
                    headers: this.headers
                }
            );

            return geoOptixRequest.data.map((x: {SiteCanonicalName: string, Name: string, Definition: {sensorType: string}}) =>
                ({wellRegistrationID: x.SiteCanonicalName, sensorName: x.Name, sensorType: SensorTypeMap[x.Definition.sensorType]})
            );

        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getSensorSummariesCreatedAsOfYear(year: number): Promise<SensorSummaryDto[]> {
        try {
            const geoOptixRequest = await axios.get(
                `${this.baseUrl}/project-overview-web/water-data-program/stations`,
                {
                    headers: this.headers
                }
            );

            const filteredSensors = geoOptixRequest.data.filter((x:any) => new Date(x.CreateDate).getFullYear() <= year);

            return filteredSensors.map((x: {SiteCanonicalName: string, Name: string, Definition: {sensorType: string}}) =>
                ({wellRegistrationID: x.SiteCanonicalName, sensorName: x.Name, sensorType: SensorTypeMap[x.Definition.sensorType]})
            );

        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getSensorsForWell(wellRegistrationID: string): Promise<SensorSummaryDto[]>{
        let geoOptixRequest;
        try {
            geoOptixRequest = await axios.get(
                `${this.baseUrl}/projects/water-data-program/sites/${wellRegistrationID}/stations`,
                {
                    headers: this.headers,
                    validateStatus: (x: number) => true
                }
            );
        } catch (err){
            console.error(err);
            throw new InternalServerError(err.message);
        }

        if (geoOptixRequest.status === 200){
            return geoOptixRequest.data.map(( x: any) =>({
                wellRegistrationID: wellRegistrationID,
                sensorName: x.CanonicalName,
                sensorType: SensorTypeMap[x.Definition.sensorType]
            }));
        } else if (geoOptixRequest.status === 404){
            return [];
        } else{
            console.error(`GeoOptix returned ${geoOptixRequest.status} for Well ${wellRegistrationID} Sensors`);
            throw new InternalServerError(`GeoOptix returned ${geoOptixRequest.status} for Well ${wellRegistrationID} Sensors`);
        }
    }

    // returns a map to facilitate quick cross-reference operations
    public async getWellsWithSensors() : Promise<Map<string,WellWithSensorSummaryDto>>{
        const sensors = await this.getSensorSummaries();
        const wells = await this.getWellSummaries();

        // add a sensor array to the wells
        const wellsWithSensors: WellWithSensorSummaryDto[] = wells.map(x=> ({
            wellRegistrationID: x.wellRegistrationID,
            description: x.description,
            location: x.location,
            sensors: [],
            inGeoOptix: true
        }));

        // create a Map from the array of wells
        const wellMap: Map<string, WellWithSensorSummaryDto> = new Map();
        wellsWithSensors.forEach(x => wellMap.set(x.wellRegistrationID, x));

        // iterate the sensor, adding each one to its well
        sensors.forEach(x => {
            const well = wellMap.get(x.wellRegistrationID);

            if (!well || !x.sensorType){
                return;
            }

            if (!well.sensors){
                well.sensors = []
            }

            well.sensors.push(x);
        });

        // return the Map for further use
        return wellMap;
    }

    public async getInstallationRecord(wellRegistrationID: string): Promise<any> {
        const installationCollectionRoute =
         `${this.baseUrl}/projects/water-data-program/sites/${wellRegistrationID}/samples`;

        try {
            const collectionResult = await axios.get(installationCollectionRoute, {
                headers: this.headers,
                validateStatus: (x: number) => true
            });

            if (collectionResult.status === 404){
                return [];
            }

            const installationRecordDtos = []

            for (const record of collectionResult.data){
                const installationCanonicalName = record?.CanonicalName;
                if (!installationCollectionRoute){
                    return null;
                }
    
                const installationResult = await axios.get(`${installationCollectionRoute}/${installationCanonicalName}/methods`, {
                    headers: this.headers
                });
    
                const installation = installationResult.data;
    
                const installationRecord = installation[0].MethodInstance.RecordSets[0].Records[0].Fields;
                const sensorRecord = installationRecord.sensor.Records[0]?.Fields;
                const photoRecords = sensorRecord.photos.Records;
      
                const sensorType = sensorRecord["sensor-type"] && (Array.isArray(sensorRecord["sensor-type"]) ? sensorRecord["sensor-type"][0] : sensorRecord["sensor-type"]);
                const continuitySensorModel = sensorRecord["sensor-model-continuity"] && (Array.isArray(sensorRecord["sensor-model-continuity"]) ? sensorRecord["sensor-model-continuity"][0] : sensorRecord["sensor-model-flow"]);
                const flowSensorModel = sensorRecord["sensor-model-flow"] && (Array.isArray(sensorRecord["sensor-model-flow"]) ? sensorRecord["sensor-model-flow"][0] : sensorRecord["sensor-model-flow"]);
                const pressureSensorModel = sensorRecord["sensor-model-pressure"] && (Array.isArray(sensorRecord["sensor-model-pressure"]) ? sensorRecord["sensor-model-pressure"][0] : sensorRecord["sensor-model-pressure"]);
      
                const installationRecordDto = {
                    installationCanonicalName: installationCanonicalName,
                    status: installation[0].Status.Name,
                    date: installationRecord["install-date"],
                    lon: installationRecord["gps-location"]?.geometry.coordinates[0],
                    lat: installationRecord["gps-location"]?.geometry.coordinates[1],
                    flowmeterSerialNumber: sensorRecord["flow-serial-number"],
                    sensorSerialNumber: sensorRecord["sensor-serial-number"],
                    affiliation: installationRecord["installer-affiliation"] && installationRecord["installer-affiliation"][0].toUpperCase(),
                    initials: installationRecord["installer-initials"],
                    sensorType: sensorType,
                    continuitySensorModel: continuitySensorModel,
                    flowSensorModel: flowSensorModel,
                    pressureSensorModel: pressureSensorModel,
                    wellDepth: sensorRecord["well-depth"],
                    installDepth: sensorRecord["install-depth"],
                    cableLength: sensorRecord["cable-length"],
                    waterLevel: sensorRecord["water-level"],
                    photos: photoRecords.map((x: any) => x.Fields.photo)
                }
                installationRecordDtos.push(installationRecordDto);
            }

            return installationRecordDtos;
        } catch(err){
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getPhoto(wellRegistrationID: string, installationCanonicalName: string, photoCanonicalName: string) {
        const photoUrl = `${this.baseUrl}/projects/water-data-program/sites/${wellRegistrationID}/samples/${installationCanonicalName}/folders/.methods/files/${photoCanonicalName}/view`

        try {
            const result = await axios.get(photoUrl, {
                headers: {
                    "x-geooptix-token": secrets.GEOOPTIX_API_KEY
                },
                responseType: "arraybuffer",
                validateStatus: (number) => true
            });

            if (result.status !== 200){
                return null;
            }

            return result.data
        } catch(err){
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getSearchSuggestions(textToSearch: string) : Promise<SearchSummaryDto[]> {
        //PageSize = -1 ensures we get as many results as Azure will let GeoOptix give us
        const searchUrl = `${this.searchUrl}/suggest/${textToSearch}?pageSize=-1`;

        try {
            const result = await axios.get(searchUrl, {
                headers: this.headers
            })

            var searchLowercase = textToSearch.toLowerCase();
            //GeoOptix search will return results where the Description or the Tags have the string
            //We're only really concerned with the Name and CanonicalName, so do some extra filtering and also make sure it isn't a new type
            return result.data.value
            .filter((x: { Document: { ObjectType: string; Name: string; CanonicalName: string }; }) => GeoOptixObjectTypeToZybachObjectType(x.Document.ObjectType) != "" && (x.Document.Name.toLowerCase().includes(searchLowercase) || x.Document.CanonicalName.toLowerCase().includes(searchLowercase)))
            .map((x: { Document: { ObjectType: string; Name: string; SiteCanonicalName: string; }; }) => 
                ({
                    ObjectType : GeoOptixObjectTypeToZybachObjectType(x.Document.ObjectType),
                    ObjectName : x.Document.Name,
                    WellID : x.Document.SiteCanonicalName
                })
            )
            .sort((a: { ObjectName: string; }, b: { ObjectName: string; }) => {
                if (a.ObjectName < b.ObjectName) {
                    return -1;
                }
                if (a.ObjectName > b.ObjectName) {
                    return  1;
                }
                return 0
            });
        }
        catch(err){
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }
}