import { SearchSummaryDto, ZybachObjectTypeEnum } from "../dtos/search-summary-dto";
import { WellSummaryDtoFactory, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { InternalServerError } from "../errors/internal-server-error";
import AghubWell, { AghubWellInterface } from "../models/aghub-well";
import { provideSingleton } from "../util/provide-singleton";

@provideSingleton(AghubWellService)
export class AghubWellService {
    public async getAghubWells(): Promise<WellWithSensorSummaryDto[]> {
        try {
            const results: AghubWellInterface[] = await AghubWell.find({});

            return results.map(x => WellSummaryDtoFactory.fromAghubWell(x));

        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }
    
    public async findByWellRegistrationID(wellRegistrationID: string): Promise<WellWithSensorSummaryDto | null> {
        try {
            const results: AghubWellInterface = await AghubWell.findOne({wellRegistrationID: wellRegistrationID});
            if (results){
                return WellSummaryDtoFactory.fromAghubWell(results);
            } else{
                return null;
            }
        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }

    public async searchByWellRegistrationID(searchString: string): Promise<SearchSummaryDto[]> {
        try {
            const results: AghubWellInterface[] = await AghubWell.find({wellRegistrationID: new RegExp(searchString, 'i')});

            return results.map(x => 
                ({
                    ObjectType : ZybachObjectTypeEnum.Well,
                    ObjectName : x.wellRegistrationID,
                    WellID : x.wellRegistrationID
                })
            )
            .sort((a: { ObjectName: string; }, b: { ObjectName: string; }) => {
                if (a.ObjectName < b.ObjectName) {
                    return -1;
                }
                if (a.ObjectName > b.ObjectName) {
                    return  1;
                }
                return 0
            });
        } catch (err) {
            console.error(err);
            throw new InternalServerError(err.message);
        }
    }
}