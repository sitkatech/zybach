import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FeatureCollection, Feature } from 'geojson';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArcService {

  constructor(private httpClient: HttpClient, handler: HttpBackend) {
    this.httpClient = new HttpClient(handler);
  }

  public getWellFromArcByRegistrationNumber(cName: any): Observable<Feature> {
    // todo: it would be more human-readable to build the query string as a dict and then URL-encode it.
    const route = `${environment.wellsLayerUrl}/query?&outFields=*&f=pgeojson&where=RegistrationNumber%3D%27${cName}%27&token=${environment.arcToken}`

    return this.httpClient.get<FeatureCollection>(route)
      .pipe(map(x => {
        // we expect exactly one feature since the well registration code is unique
        return x.features[0];
      }));
  }
}

// todo: the source layer for these features keeps changing and the schema with it. Eventually needs to be locked down, until then there might be an occasional need to re-write this function
export function remapWellFeaturePropertiesFromArc(feature: Feature): any {
  return {
    Acres: feature.properties.Acres,
    CompletionDate: feature.properties.CompletionDate,
    CountyName: feature.properties.CountyName,
    DecommissionedDate: feature.properties.DecommissionedDate,
    FirstName: feature.properties.FirstName,
    LastChangeDate: feature.properties.LastChangeDate,
    LastName: feature.properties.LastName,
    Latitude: feature.properties.Latitude,
    LatitudeDMS: feature.properties.LatitudeDMS,
    Location: feature.properties.Location,
    Longitude: feature.properties.Longitude,
    LongitudeDMS: feature.properties.LongitudeDMS,
    NRDName: feature.properties.NRDName,
    NRDPermit: feature.properties.NRDPermit,
    ObjectId: feature.properties.ObjectId,
    OwnerID: feature.properties.OwnerID,
    PumpColumn_Diameter: feature.properties.PumpColumn_Diameter,
    PumpDepth: feature.properties.PumpDepth,
    PumpRate: feature.properties.PumpRate,
    PumpingWaterLevel: feature.properties.PumpingWaterLevel,
    RegistrationDate: feature.properties.RegistrationDate,
    RegistrationNumber: feature.properties.RegistrationNumber,
    SeriesType: feature.properties.SeriesType,
    StaticWaterLevel: feature.properties.StaticWaterLevel,
    Status: feature.properties.Status,
    TotalDepth: feature.properties.TotalDepth,
    WellID: feature.properties.WellID,
    WellUseDescription: feature.properties.WellUseDescription,
  }
}


// export function remapWellFeaturePropertiesFromArc(feature: Feature): any {
//   debugger;
//   return {
//     FID: feature.properties.FID,
//     OBJECTID: feature.properties.Active_Irr,
//     WellID: feature.properties.Active_I_1,
//     RegistrationNumber: feature.properties.Active_I_2,
//     Replacemen: feature.properties.Active_I_3,
//     Status: feature.properties.Active_I_4,
//     Useid: feature.properties.Active_I_5,
//     NrdName: feature.properties.Active_I_6,
//     NrdID: feature.properties.Active_I_7,
//     Countyname: feature.properties.Active_I_8,
//     CountyID: feature.properties.Active_I_9,
//     Township: feature.properties.Active__10,
//     Range: feature.properties.Active__11,
//     RangeDir: feature.properties.Active__12,
//     Section_: feature.properties.Active__13,
//     SubSection: feature.properties.Active__14,
//     FootageNS: feature.properties.Active__15,
//     FootageDir: feature.properties.Active__16,
//     FootageEW: feature.properties.Active__17,
//     FootageD_1: feature.properties.Active__18,
//     NrdPermit: feature.properties.Active__19,
//     Acres: feature.properties.Active__20,
//     SeriesType: feature.properties.Active__21,
//     SeriesEnd: feature.properties.Active__22,
//     PumpRate: feature.properties.Active__23,
//     PColDiam: feature.properties.Active__24,
//     PumpDepth: feature.properties.Active__25,
//     TotalDepth: feature.properties.Active__26,
//     SWL: feature.properties.Active__27,
//     PWL: feature.properties.Active__28,
//     CertifID: feature.properties.Active__29,
//     OwnerID: feature.properties.Active__30,
//     FirstName: feature.properties.Active__31,
//     LastName: feature.properties.Active__32,
//     Address: feature.properties.Active__33,
//     CityNameID: feature.properties.Active__34,
//     StateRID: feature.properties.Active__35,
//     PostalCD: feature.properties.Active__36,
//     RegDate: feature.properties.Active__37,
//     Compdate: feature.properties.Active__38,
//     LastChgDat: feature.properties.Active__39,
//     DecommDate: feature.properties.Active__40,
//     LatDD: feature.properties.Active__41,
//     LongDD: feature.properties.Active__42,
//     CalcGPS: feature.properties.Active__43,
//     Hyperlink: feature.properties.Active__44,
//     tpid_OBJECTID: feature.properties.tpid_wells,
//     tpid_regcd: feature.properties.tpid_wel_1,
//     tpid: feature.properties.tpid_wel_2
//   }
// }