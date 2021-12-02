import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { DefaultBoundingBox } from '../shared/models/default-bounding-box';

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
    const boundingBox = DefaultBoundingBox;
    const url: string = `${this.baseURL}&format=json&q=${q}&viewbox=${boundingBox.Right},${boundingBox.Top},${boundingBox.Left},${boundingBox.Bottom}&bounded=1`;
    return this.http.get<any>(url);
  }
}