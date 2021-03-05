import { inject } from "inversify";
import { Route, Controller, Get, Hidden, Security, Path } from "tsoa";
import { SecurityType } from "../security/authentication";
import { GeoOptixService } from "../services/geooptix-service";
import { provideSingleton } from "../util/provide-singleton";

@Route("/api/search")
@provideSingleton(SearchController)
export class SearchController extends Controller {
    constructor(@inject(GeoOptixService) private geoOptixService: GeoOptixService) {
        super();
    }

    @Get("{searchText}")
    @Hidden()
    @Security(SecurityType.API_KEY)
    public async getSearchSuggestions(
        @Path() searchText: string
    ) {
        const results = await this.geoOptixService.getSearchSuggestions(searchText);

        return results;
    }
}