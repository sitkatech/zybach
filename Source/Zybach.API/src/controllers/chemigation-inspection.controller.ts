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

@Route("/api/chemigation")
@provideSingleton(ChemigationInspectionController)
export class ChemigationInspectionController extends Controller {
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
    ) {
        super();
    }


    @Get("/summaries")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getInspectionSummaries(): Promise<any> {
        return [];
    }
}
