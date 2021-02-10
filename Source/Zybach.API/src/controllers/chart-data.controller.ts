import { inject } from "inversify";
import { Hidden, Route, Controller, Get, Path } from "tsoa";
import { InfluxService } from "../services/influx-service";
import { provideSingleton } from "../util/provide-singleton";

@Hidden()
@Route("/api/chartData")
@provideSingleton(ChartDataController)
export class ChartDataController extends Controller{
    constructor(
        @inject(InfluxService) private influxService: InfluxService
    ) {
        super();
    }

    @Get("/electricalBasedEstimate/{wellRegistrationID}")
    public async getElectricalBasedFlowEstimateSeries(
        @Path() wellRegistrationID: string
    ) {
        return await this.influxService.getElectricalBasedFlowEstimateSeries(wellRegistrationID);
    }
}