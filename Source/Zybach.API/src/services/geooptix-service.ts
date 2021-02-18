import axios from "axios";
import { InternalServerError } from "../errors/internal-server-error";
import secrets from "../secrets";
import { provideSingleton } from "../util/provide-singleton";
import { SensorSummaryDto, SensorTypeMap, WellSummaryDto, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";

@provideSingleton(GeoOptixService)
export class GeoOptixService {
    public async getWellSummaries(): Promise<WellSummaryDto[]> {
        try {
            // todo: this getting stuff from GeoOptix needs to live in GeoOptixService class
            const geoOptixRequest = await axios.get(
                `${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites`,
                {
                    headers: {
                        "x-geooptix-token": secrets.GEOOPTIX_API_KEY
                    }
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

    public async getSensorSummaries(): Promise<SensorSummaryDto[]> {
        try {
            const geoOptixRequest = await axios.get(
                `${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/stations`,
                {
                    headers: {
                        "x-geooptix-token": secrets.GEOOPTIX_API_KEY
                    }
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

    public async getSensorsForWell(wellRegistrationID: string): Promise<SensorSummaryDto[]>{
        let geoOptixRequest;
        try {
            geoOptixRequest = await axios.get(
                `${secrets.GEOOPTIX_HOSTNAME}/projects/water-data-program/sites/${wellRegistrationID}/stations`,
                {
                    headers: {
                        "x-geooptix-token": secrets.GEOOPTIX_API_KEY
                    },
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
}