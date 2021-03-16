import { inject } from "inversify";
import { Hidden, Route, Controller, Get, Path, Security } from "tsoa";
import { AnnualPumpedVolumeDto } from "../dtos/annual-pumped-volume-dto";
import { WellDetailDto, WellSummaryDto, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { AghubWellService } from "../services/aghub-well-service";
import { GeoOptixService } from "../services/geooptix-service";
import { InfluxService } from "../services/influx-service";
import { provideSingleton } from "../util/provide-singleton";

@Hidden()
@Route("/api/chartData")
@provideSingleton(ChartDataController)
export class ChartDataController extends Controller {
    constructor(
        @inject(InfluxService) private influxService: InfluxService,
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(AghubWellService) private aghubWellService: AghubWellService
    ) {
        super();
    }

    @Get("{wellRegistrationID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getChartData(
        @Path() wellRegistrationID: string
    ) {
        const firstReadingDate = await this.influxService.getFirstReadingDateTimeForWell(wellRegistrationID);

        if (!firstReadingDate) {
            //this.setStatus(204);
            return { timeSeries: null, sensors: null };
        }

        const sensors = await this.geooptixService.getSensorsForWell(wellRegistrationID);

        const agHubWell = await this.aghubWellService.findByWellRegistrationID(wellRegistrationID);
        const hasElectricalData = agHubWell && agHubWell.hasElectricalData;

        let timeSeriesPoints: any[] = []

        for (var sensor of sensors) {
            const sensorPoints = await this.influxService.getPumpedVolumeForSensor(sensor, firstReadingDate)

            let gallons = 0;
            for (var obs of sensorPoints){
                gallons += obs.gallons;
            }

            timeSeriesPoints = [...timeSeriesPoints, ...sensorPoints];
        }

        if (hasElectricalData) {
            sensors.push({ wellRegistrationID: wellRegistrationID, sensorType: "Electrical Data" });
            const electricPoints = await this.influxService.getElectricalBasedFlowEstimateSeries(wellRegistrationID, firstReadingDate);
            timeSeriesPoints = [...timeSeriesPoints, ...electricPoints];
        }

        timeSeriesPoints.forEach(x => {
            x.gallonsString = x.gallons.toLocaleString() + " gallons"
        })

        return { timeSeries: timeSeriesPoints, sensors: sensors };
    }
}