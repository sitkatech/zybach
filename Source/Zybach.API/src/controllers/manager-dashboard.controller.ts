import { inject } from "inversify";
import { Hidden, Route, Controller, Get, Path } from "tsoa";
import { DistrictStatisticsDto } from "../dtos/district-statistics-dto";
import { InternalServerError } from "../errors/internal-server-error";
import { GeoOptixService } from "../services/geooptix-service";
import { InfluxService } from "../services/influx-service";
import { provideSingleton } from "../util/provide-singleton";

@Hidden()
@Route("/api/managerDashboard")
@provideSingleton(ManagerDashboardController)
export class ManagerDashboardController extends Controller{
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(InfluxService) private influxService: InfluxService
    ){
        super();
    }

    @Get("/districtStatistics/{year}")
    
    public async getDistrictStatistics(
        @Path() year: number
    ) : Promise<DistrictStatisticsDto> {
        let numberOfWellsTracked, numberOfContinuityMeters, numberOfFlowMeters, numberOfElectricalUsageEstimates;

        // get all wells that either existed in GeoOptix as of the given year or that had electrical estimates as of the given year
        const geoOptixWells = await this.geooptixService.getWellSummariesCreatedAsOfYear(year);
        const aghubRegistrationIds = await this.influxService.getWellRegistrationIdsWithElectricalEstimateAsOfYear(year);
        // combine the registration ids as a set and count to avoid counting duplicates
        numberOfWellsTracked = new Set([...geoOptixWells.map(x=>x.wellRegistrationID), ...aghubRegistrationIds]).size;
        
        // get all sensors that existed in GeoOptix as of the given year
        const sensors = await this.geooptixService.getSensorSummariesCreatedAsOfYear(year);
        
        // filter by sensor type and count
        numberOfFlowMeters = sensors.filter(x=>x.sensorType === 'Flow Meter').length;
        numberOfContinuityMeters = sensors.filter(x=>x.sensorType === 'Continuity Meter').length;

        // todo: get total number of electrical usage estimates
        numberOfElectricalUsageEstimates = aghubRegistrationIds.length;

        return {
            NumberOfWellsTracked: numberOfWellsTracked,
            NumberOfContinuityMeters: numberOfContinuityMeters,
            NumberOfElectricalUsageEstimates: numberOfElectricalUsageEstimates,
            NumberOfFlowMeters: numberOfFlowMeters
        }
    }
}