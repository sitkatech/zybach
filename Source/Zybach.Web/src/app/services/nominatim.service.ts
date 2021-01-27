import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class NominatimService {

  private baseURL = environment.mapQuestApiUrl;

  constructor(
    private http: HttpClient,
  ) {
  }

  public makeNominatimRequest(q: string): Observable<any> {
    const url: string = `${this.baseURL}&format=json&q=${q}&viewbox=-100.2234130905508,41.743487069127966,-102.05591553769607,40.74358623146166&bounded=1`;
    return this.http.get<any>(url);
  }
}