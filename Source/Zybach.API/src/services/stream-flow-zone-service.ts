import { InternalServerError } from "../errors/internal-server-error";
import StreamFlowZone, { StreamFlowZoneInterface } from "../models/stream-flow-zone";
import { provideSingleton } from "../util/provide-singleton";

@provideSingleton(StreamFlowZoneService)
export class StreamFlowZoneService {
    constructor(){}

    public async findAll():Promise<StreamFlowZoneInterface[]> {
        try {
            const results: StreamFlowZoneInterface[] = await StreamFlowZone.find({});

            //return results.map(x => WellSummaryDtoFactory.fromAghubWell(x));
            return results;

        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }
}