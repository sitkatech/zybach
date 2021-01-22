import { Controller, Get, Hidden, Route, Security } from "tsoa";
import { ApiResult } from "../dtos/api-result";
import { WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { GeoOptixService } from "../services/geooptix-service";

@Hidden()
@Route("/api/mapData")
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