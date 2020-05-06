import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ArcService {


  constructor(private httpClient: HttpClient, handler: HttpBackend) {
    this.httpClient = new HttpClient(handler);
  }



  public getWellFromArcByRegCD(cName: any) {
    const route = `${environment.wellsLayerUrl}/query?&outFields=*&f=pgeojson&where=Active_I_2%3D%27${cName}%27&token=${environment.arcToken}`

    return this.httpClient.get<any>(route);
  }
}
