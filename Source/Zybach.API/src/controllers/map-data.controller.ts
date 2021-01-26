import { Controller, Get, Hidden, Route, Security } from "tsoa";
import { provideSingleton } from "../util/provide-singleton";
import { ApiResult } from "../dtos/api-result";
import { WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { GeoOptixService } from "../services/geooptix-service";

@Hidden()
@Route("/api/mapData")
@provideSingleton(MapDataController)
export class MapDataController extends Controller{
    @Get("wells")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getWellsWithSensors(): Promise<ApiResult<WellWithSensorSummaryDto[]>>{
        const wellSummaryWithSensorsDtos = await new GeoOptixService().getWellsWithSensors();

        return {
            "status": "success",
            "result": wellSummaryWithSensorsDtos
        };
    }
}