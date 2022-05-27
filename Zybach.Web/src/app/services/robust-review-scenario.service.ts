import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RobustReviewScenarioGETRunHistoryDto } from '../shared/generated/model/robust-review-scenario-get-run-history-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class RobustReviewScenarioService {
  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient
  ) { }

  public checkGETAPIHealth(): Observable<boolean> {
    return this.apiService.getFromApi("robustReviewScenario/checkGETAPIHealth");
  }

  public getRobustReviewScenarioGETRunHistories(): Observable<RobustReviewScenarioGETRunHistoryDto[]> {
    return this.apiService.getFromApi("robustReviewScenarios");
  }

  public getRobustReviewScenarioJson(): Observable<any> {
    const route = `${environment.mainAppApiUrl}/robustReviewScenario/download/robustReviewScenarioJson`;
    return this.httpClient.get(route, { responseType: "blob" as "json" });
  }

  public newRobustReviewScenarioGETHistory(): Observable<any> {
    let route = `/robustReviewScenario/new`;
    return this.apiService.postToApi(route, null);
  }
}
