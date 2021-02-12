import { Controller, Get, Hidden, Route, Security } from "tsoa";
import { provideSingleton } from "../util/provide-singleton";
import { ApiResult } from "../dtos/api-result";
import { WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { GeoOptixService } from "../services/geooptix-service";
import { inject } from "inversify";
import { InfluxService } from "../services/influx-service";
import { Stopwatch } from "ts-stopwatch";
import { AghubWellService } from "../services/aghub-well-service";

@Hidden()
@Route("/api/mapData")
@provideSingleton(MapDataController)
export class MapDataController extends Controller{
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(InfluxService) private influxService: InfluxService,
        @inject(AghubWellService) private aghubWellService: AghubWellService
    ){
        super();
    }

    @Get("wells")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getWellsWithSensors(): Promise<ApiResult<WellWithSensorSummaryDto[]>>{
        const sw = new Stopwatch();
        sw.start();
        const wellSummaryWithSensorsDtoMap = await this.geooptixService.getWellsWithSensors();
        const lastReadingDates = await this.influxService.getLastReadingDateTime();
        const firstReadingDates = await this.influxService.getFirstReadingDateTime();
        const aghubWells = await this.aghubWellService.getAghubWells();
        console.log(sw.getTime());

        // iterate the aghub wells
        // if the well summary map does not have a well with a given wellRegistrationID, add it
        // otherwise, append the sensors from the aghub well to the existing record
        aghubWells.forEach(x=> {
            const geoOptixWell = wellSummaryWithSensorsDtoMap.get(x.wellRegistrationID);
            if (!geoOptixWell){
                wellSummaryWithSensorsDtoMap.set(x.wellRegistrationID, x);
                return;
            }

            geoOptixWell.sensors = [...geoOptixWell.sensors, ...x.sensors];
            geoOptixWell.wellTPID = x.wellTPID;
            geoOptixWell.fetchDate = x.fetchDate;
        })

        console.log(sw.getTime());

        // iterate the combined collection, setting the last reading date for each well
        wellSummaryWithSensorsDtoMap.forEach(x=>{
            x.lastReadingDate = lastReadingDates[x.wellRegistrationID]
            x.firstReadingDate = firstReadingDates[x.wellRegistrationID]
        })

        console.log(sw.getTime());

        const resultArray = Array.from(wellSummaryWithSensorsDtoMap.values())

        return {
            "status": "success",
            "result": resultArray
        };
    }
}