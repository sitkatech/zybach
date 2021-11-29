import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiRouteService {

  constructor() {
  }

  getRoute(): string {
    const mainAppApiUrl = `${environment.mainAppApiUrl}`;
    return mainAppApiUrl;
  }
}
