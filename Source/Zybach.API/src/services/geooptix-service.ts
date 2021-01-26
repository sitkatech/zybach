import axios from "axios";
import { InternalServerError } from "../errors/internal-server-error";
import secrets from "../secrets";
import { provideSingleton } from "../util/provide-singleton";
import { SensorSummaryDto, WellSummaryDto, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";

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
                ({wellRegistrationID: x.SiteCanonicalName, sensorName: x.Name, sensorType: x.Definition.sensorType})
            );

        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async getWellsWithSensors() : Promise<WellWithSensorSummaryDto[]>{
        const sensors = await this.getSensorSummaries();
        const wells = await this.getWellSummaries();

        const wellsWithSensors: WellWithSensorSummaryDto[] = wells.map(x=> ({
            wellRegistrationID: x.wellRegistrationID,
            description: x.description,
            location: x.location,
            sensors: []
        }))

        const wellMap: {[key: string]: WellWithSensorSummaryDto} = {};
        wellsWithSensors.forEach(x => wellMap[x.wellRegistrationID] = x);

        sensors.forEach(x => {
            const well = wellMap[x.wellRegistrationID];
            if (!well.sensors){
                well.sensors = []
            }

            well.sensors.push(x);
        });

        return wellsWithSensors;
    }
}