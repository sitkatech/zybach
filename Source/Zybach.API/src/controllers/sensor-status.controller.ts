import {
    Controller,
    Get,
    Path,
    Query,
    Route,
    Security,
    Response,
    Request,
    SuccessResponse,
    Hidden
} from "tsoa";
import secrets from '../secrets';
import { ApiError } from '../errors/apiError'
import axios from 'axios';
import moment from 'moment';
import { InfluxDB } from '@influxdata/influxdb-client'
import { SecurityType } from "../security/authentication";
import { WellDetailDto, WellSummaryDto, WellWithSensorMessageAgeDto, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { ApiResult, ErrorResult } from "../dtos/api-result";
import { GeoOptixService } from "../services/geooptix-service";
import { InternalServerError } from "../errors/internal-server-error";
import { provideSingleton } from "../util/provide-singleton";
import { inject } from "inversify";
import { RequestWithUserContext } from "../request-with-user-context";
import { RoleEnum } from "../models/role";
import { AghubWellService } from "../services/aghub-well-service";
import { InfluxService } from "../services/influx-service";
import { AnnualPumpedVolumeDto } from "../dtos/annual-pumped-volume-dto";
import { RobustReviewDto, MonthlyPumpedVolumeGallonsDto} from "../dtos/robust-review-dto";
import { NotFoundError } from "../errors/not-found-error";
import SensorMessageAgeDto from "../dtos/sensor-message-age-dto";

const bucketName = process.env.SOURCE_BUCKET;

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
    @Security(SecurityType.ANONYMOUS)
    public async getSensorMessageAges(): Promise<WellWithSensorMessageAgeDto[]> {
        const wellSummariesWithSensors = Array.from((await this.geooptixService.getWellsWithSensors()).values()).filter(x=>x.sensors.length);
        const sensorMessageAges = await this.influxService.getLastMessageAgeBySensor();

        return wellSummariesWithSensors.map(well=>({
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
        }))
    }
 
}