import { inject } from "inversify";
import { Controller, Route, Security, Get, Hidden } from "tsoa";
import { provideSingleton } from "../util/provide-singleton";
import { SecurityType } from "../security/authentication";
import { StreamFlowZoneInterface } from "../models/stream-flow-zone";
import { StreamFlowZoneService } from "../services/stream-flow-zone-service";


@Route("/api/streamFlowZones")
@provideSingleton(StreamFlowZoneController)
@Hidden()
export class StreamFlowZoneController extends Controller{
    constructor(@inject(StreamFlowZoneService) private streamflowZoneService: StreamFlowZoneService){
        super();
    }

    @Get("")
    @Security(SecurityType.ANONYMOUS)
    public async getStreamflowZones() : Promise<StreamFlowZoneInterface[]>{
        const newBook = await this.streamflowZoneService.findAll();
        return newBook;        
    }
}