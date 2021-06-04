import {
    Controller,
    Get,
    Route,
    Security
} from "tsoa";
import { SecurityType } from "../security/authentication";
import { WellWithSensorMessageAgeDto } from "../dtos/well-summary-dto";
import { GeoOptixService } from "../services/geooptix-service";
import { provideSingleton } from "../util/provide-singleton";
import { inject } from "inversify";
import { InfluxService } from "../services/influx-service";
import { RoleEnum } from "../models/role";
import { ChemigationInspectionService } from "../services/chemigation-inspection-service";
import { ChemigationInspectionDto } from "../dtos/chemigation-inspection-dto";
import { WellInspectionSummaryDto } from "../dtos/well-inspection-summary-dto";

@Route("/api/chemigation")
@provideSingleton(ChemigationInspectionController)
export class ChemigationInspectionController extends Controller {
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(ChemigationInspectionService) private chemigationInspectionService: ChemigationInspectionService
    ) {
        super();
    }


    @Get("/summaries")
    @Security(SecurityType.ANONYMOUS, [RoleEnum.Adminstrator])
    public async getInspectionSummaries(): Promise<WellInspectionSummaryDto[]> {
        const inspections = await this.chemigationInspectionService.getChemigationInspections();
        const wells = await this.geooptixService.getWellSummaries();

        const inspectionsMap = new Map<string, ChemigationInspectionDto[]>();
        for (const well of wells) {
            inspectionsMap.set(well.wellRegistrationID, []);
        }

        for (const inspection of inspections) {
            inspectionsMap.get(inspection.wellRegistrationID)?.push(inspection);
        }

        return Array.from(inspectionsMap).map(([wellRegistrationID, inspections]) => {
            const lastChemigationDate = inspections.filter(x => x.protocolCanonicalName === 'chemigation-inspection')
                .map(x => x.lastUpdate).sort((a, b) => b.getTime() - a.getTime())[0];
            const lastNitratesDate = inspections.filter(x => x.protocolCanonicalName === 'nitrates-inspection')
                .map(x => x.lastUpdate).sort((a, b) => b.getTime() - a.getTime())[0];
            const lastWaterLevelDate = inspections.filter(x => x.protocolCanonicalName === 'water-level-inspection')
                .map(x => x.lastUpdate).sort((a, b) => b.getTime() - a.getTime())[0];
            const lastWaterQualitydate = inspections.filter(x => x.protocolCanonicalName === 'water-quality-inspection')
                .map(x => x.lastUpdate).sort((a, b) => b.getTime() - a.getTime())[0];

            const pendingInspectionsCount = inspections.filter(x => x.status !== "Approved").length;

            return {
                wellRegistrationID, lastChemigationDate, lastNitratesDate, lastWaterLevelDate, lastWaterQualitydate, pendingInspectionsCount
            }
        })
    }
}
