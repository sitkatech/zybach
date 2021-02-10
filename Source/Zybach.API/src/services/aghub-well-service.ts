import { WellSummaryDtoFactory } from "../dtos/well-summary-dto";
import { InternalServerError } from "../errors/internal-server-error";
import AghubWell, { AghubWellInterface } from "../models/aghub-well";
import { provideSingleton } from "../util/provide-singleton";

@provideSingleton(AghubWellService)
export class AghubWellService {
    public async getAghubWells() {
        try {
            const results: AghubWellInterface[] = await AghubWell.find({});

            return results.map(x => WellSummaryDtoFactory.fromAghubWell(x));

        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }
}