import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(
    public apiService: ApiService
  ) { }

  public getSearchSuggestions(searchText : string): Observable<any> {
    return this.apiService.getFromApi(`search/${searchText}`);
  }
}
