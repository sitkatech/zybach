import { inject } from "inversify";
import { Hidden, Route, Controller, Get, Path } from "tsoa";
import { AghubWellService } from "../services/aghub-well-service";
import { GeoOptixService } from "../services/geooptix-service";
import { InfluxService } from "../services/influx-service";
import { provideSingleton } from "../util/provide-singleton";

@Hidden()
@Route("/api/chartData")
@provideSingleton(ChartDataController)
export class ChartDataController extends Controller{
    constructor(
        @inject(InfluxService) private influxService: InfluxService,
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(AghubWellService) private aghubWellService: AghubWellService
    ) {
        super();
    }

    @Get("electricalBasedEstimate/{wellRegistrationID}")
    public async getElectricalBasedFlowEstimateSeries(
        @Path() wellRegistrationID: string
    ) {
        const firstReadingDate = await this.influxService.getFirstReadingDateTimeForWell(wellRegistrationID);
        return await this.influxService.getElectricalBasedFlowEstimateSeries(wellRegistrationID, firstReadingDate);
    }

    @Get("{wellRegistrationID}")
    public async getChartData(
        @Path() wellRegistrationID: string
    ){
        const sensors = await this.geooptixService.getSensorsForWell(wellRegistrationID);
        const agHubWell = await this.aghubWellService.findByWellRegistrationID(wellRegistrationID);
        const firstReadingDate = await this.influxService.getFirstReadingDateTimeForWell(wellRegistrationID);
        if (!firstReadingDate){
            this.setStatus(204);
            return {timeSeries: null, sensors: null};
        }
        const hasElectricalData = agHubWell && agHubWell.wellConnectedMeter;

        let timeSeriesPoints: any[] = []

        for (var sensor of sensors){
            const sensorPoints = await this.influxService.getPumpedVolumeForSensor(sensor, firstReadingDate)
            timeSeriesPoints = [...timeSeriesPoints, ...sensorPoints];
        }

        if (hasElectricalData){
            sensors.push({ wellRegistrationID: wellRegistrationID, sensorType: "Electrical Data" });
            const electricPoints = await this.influxService.getElectricalBasedFlowEstimateSeries(wellRegistrationID, firstReadingDate);
            timeSeriesPoints = [...timeSeriesPoints, ...electricPoints];
        }

        timeSeriesPoints.forEach(x=>{
            x.gallonsString = x.gallons.toLocaleString() + " gallons"
        })

        return {timeSeries: timeSeriesPoints, sensors: sensors}; 
    }
}