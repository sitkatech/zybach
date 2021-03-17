import { inject } from "inversify";
import { Route, Controller, Get, Hidden, Security, Path } from "tsoa";
import { SecurityType } from "../security/authentication";
import { AghubWellService } from "../services/aghub-well-service";
import { GeoOptixService } from "../services/geooptix-service";
import { provideSingleton } from "../util/provide-singleton";

@Route("/api/search")
@provideSingleton(SearchController)
export class SearchController extends Controller {
    constructor(
        @inject(GeoOptixService) private geoOptixService: GeoOptixService,
        @inject(AghubWellService) private aghubWellService: AghubWellService) {
        super();
    }

    @Get("{searchText}")
    @Hidden()
    @Security(SecurityType.API_KEY)
    public async getSearchSuggestions(
        @Path() searchText: string
    ) {
        const geoOptixResults = await this.geoOptixService.getSearchSuggestions(searchText);
        const aghubResults = await this.aghubWellService.searchByWellRegistrationID(searchText);
        const results = [...new Set([...geoOptixResults,...aghubResults.filter(x => !geoOptixResults.some(y => y.ObjectName === x.ObjectName))])].sort((a, b) => {
            //Used to counteract the 'Object is possibly undefined' error
            if (a != null && a.ObjectName != null && b != null && b.ObjectName != null) {
                if (a.ObjectName < b.ObjectName) {
                    return -1;
                }
                if (a.ObjectName > b.ObjectName) {
                    return  1;
                }
            }
            return 0
        });


        return results;
    }
}