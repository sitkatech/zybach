import { inject } from "inversify";
import { Hidden, Route, Controller, Get, Path, Security } from "tsoa";
import { DistrictStatisticsDto } from "../dtos/district-statistics-dto";
import { streamFlowZonePumpingDepthDto } from "../dtos/stream-flow-zone-pumping-depth-dto";
import { InternalServerError } from "../errors/internal-server-error";
import AghubWell from "../models/aghub-well";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { AghubWellService } from "../services/aghub-well-service";
import { GeoOptixService } from "../services/geooptix-service";
import { InfluxService } from "../services/influx-service";
import GALLON_TO_ACRE_INCH from "../util/constants";
import { provideSingleton } from "../util/provide-singleton";

@Hidden()
@Route("/api/managerDashboard")
@provideSingleton(ManagerDashboardController)
export class ManagerDashboardController extends Controller {
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(InfluxService) private influxService: InfluxService,
        @inject(AghubWellService) private aghubWellService: AghubWellService
    ) {
        super();
    }

    @Get("/districtStatistics/{year}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getDistrictStatistics(
        @Path() year: number
    ): Promise<DistrictStatisticsDto> {
        let numberOfWellsTracked, numberOfContinuityMeters, numberOfFlowMeters, numberOfElectricalUsageEstimates;

        // get all wells that either existed in GeoOptix as of the given year or that had electrical estimates as of the given year
        const geoOptixWells = await this.geooptixService.getWellSummariesCreatedAsOfYear(year);
        const aghubRegistrationIds = await this.influxService.getWellRegistrationIdsWithElectricalEstimateAsOfYear(year);
        // combine the registration ids as a set and count to avoid counting duplicates
        numberOfWellsTracked = new Set([...geoOptixWells.map(x => x.wellRegistrationID), ...aghubRegistrationIds]).size;

        // get all sensors that existed in GeoOptix as of the given year
        const sensors = await this.geooptixService.getSensorSummariesCreatedAsOfYear(year);

        // filter by sensor type and count
        numberOfFlowMeters = sensors.filter(x => x.sensorType === 'Flow Meter').length;
        numberOfContinuityMeters = sensors.filter(x => x.sensorType === 'Continuity Meter').length;

        // todo: get total number of electrical usage estimates
        numberOfElectricalUsageEstimates = aghubRegistrationIds.length;

        return {
            NumberOfWellsTracked: numberOfWellsTracked,
            NumberOfContinuityMeters: numberOfContinuityMeters,
            NumberOfElectricalUsageEstimates: numberOfElectricalUsageEstimates,
            NumberOfFlowMeters: numberOfFlowMeters
        }
    }

    @Get("/streamFlowZonePumpingDepths")
    public async getStreamFlowZonePumpingDepths(
    ): Promise<{ year: number, streamFlowZonePumpingDepths: streamFlowZonePumpingDepthDto[] }[]> {
        // Currently, we are only accounting for electrical data when color-coding the SFZ map;
        // hence, we can confine our attention to the aghub wells and the electrical estimate time series

        const currentYear = new Date().getFullYear();
        const years = []

        for (let year = 2019; year <= currentYear; year++) {
            years.push(year);
        }

        // Step 1. Get a mapping from SFZs to Wells
        const streamFlowZoneWellMap = await this.aghubWellService.getAllWellsWithStreamFlowZones();
        const results = await Promise.all(years.map(async year => {
            // Step 2. Get the total pumped volume for each well.
            // This is represented as a mapping from Well Registration IDs to pumped volumes
            let pumpedVolumes: Map<string, number> = await this.influxService.getAnnualEstimatedPumpedVolumeByWellForYear(year);

            // Step 3. For each SFZ, calculate the pumping depth
            const pumpingDepths: streamFlowZonePumpingDepthDto[] = [];
            for (const [zone, wells] of streamFlowZoneWellMap) {
                if (!wells?.length) {
                    pumpingDepths.push({ streamFlowZoneFeatureID: zone.properties.FeatureID, pumpingDepth: 0, totalIrrigatedAcres: 0, totalPumpedVolume:0 });
                    continue;
                }

                // Sum the irrigated acres and pumped volume for each well
                const totals = wells.reduce((runningTotals, currentWell) => {
                    const wellAcres = currentWell.irrigatedAcresPerYear.find(x => x.year === year)?.acres ?? 0
                    const wellPumpedVolume = pumpedVolumes.get(currentWell.wellRegistrationID) ?? 0;

                    return {
                        totalAcres: runningTotals.totalAcres + wellAcres,
                        totalVolume: runningTotals.totalVolume + wellPumpedVolume
                    }
                }, { totalAcres: 0, totalVolume: 0 })

                // todo: this is reporting in gallons/acres right now and we probably want acre-inch per acre
                pumpingDepths.push({ streamFlowZoneFeatureID: zone.properties.FeatureID, pumpingDepth: GALLON_TO_ACRE_INCH * totals.totalVolume / totals.totalAcres, totalIrrigatedAcres: totals.totalAcres, totalPumpedVolume: GALLON_TO_ACRE_INCH * totals.totalVolume })
            }

            return { year, streamFlowZonePumpingDepths: pumpingDepths };
        }));

        return results;
    }
}