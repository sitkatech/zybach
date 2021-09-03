import {
    Controller,
    Get,
    Route,
    Security
} from "tsoa";
import { SecurityType } from "../security/authentication";
import { WellWithSensorMessageAgeDto } from "../dtos/well-summary-dto";
import { GeoOptixService } from "../services/geooptix-service";
import { provideSingleton } from "../util/provide-singleton";
import { inject } from "inversify";
import { InfluxService } from "../services/influx-service";
import { RoleEnum } from "../models/role";

@Route("/api/sensorStatus")
@provideSingleton(SensorStatusController)
export class SensorStatusController extends Controller {
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(InfluxService) private influxService: InfluxService,
    ) {
        super();
    }


    @Get("")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getSensorMessageAges(): Promise<WellWithSensorMessageAgeDto[]> {
        const wellSummariesWithSensors = Array.from((await this.geooptixService.getWellsWithSensors()).values()).filter(x => x.sensors.length);
        const sensorMessageAges = await this.influxService.getLastMessageAgeBySensor();

        return wellSummariesWithSensors.map(well => ({
            wellRegistrationID: well.wellRegistrationID,
            location: well.location,
            sensors: well.sensors.map(sensor => {
                const messageAge = sensorMessageAges.get(sensor.sensorName as string)
                return {
                    sensorName: sensor.sensorName as string,
                    messageAge: messageAge ?? null,
                    sensorType: sensor.sensorType
                }
            })
        }));
    }
}
