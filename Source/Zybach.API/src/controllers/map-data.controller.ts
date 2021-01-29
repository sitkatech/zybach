import { Controller, Get, Hidden, Route, Security } from "tsoa";
import { provideSingleton } from "../util/provide-singleton";
import { ApiResult } from "../dtos/api-result";
import { WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { GeoOptixService } from "../services/geooptix-service";
import { inject } from "inversify";
import { InfluxService } from "../services/influx-service";

@Hidden()
@Route("/api/mapData")
@provideSingleton(MapDataController)
export class MapDataController extends Controller{
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(InfluxService) private influxService: InfluxService
    ){
        super();
    }

    @Get("wells")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getWellsWithSensors(): Promise<ApiResult<WellWithSensorSummaryDto[]>>{
        const wellSummaryWithSensorsDtos = await this.geooptixService.getWellsWithSensors();
        const lastReadingDates = await this.influxService.getLastReadingDatetime();

        wellSummaryWithSensorsDtos.forEach(x=>{
            x.lastReadingDate = lastReadingDates[x.wellRegistrationID]
        })

        return {
            "status": "success",
            "result": wellSummaryWithSensorsDtos
        };
    }
}