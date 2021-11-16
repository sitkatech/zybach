import { Injectable } from '@angular/core';
import { ApiService } from '.';
import { Observable } from 'rxjs';
import { ReportTemplateDto } from '../models/generated/report-template-dto';
import { ReportTemplateUpdateDto } from '../models/report-template-update-dto';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { GenerateReportsDto } from '../models/generate-reports-dto';

@Injectable({
  providedIn: 'root'
})
export class ReportTemplateService {

  constructor(private apiService: ApiService, private httpClient: HttpClient) { }

  public listAllReportTemplates(): Observable<Array<ReportTemplateDto>> {
    return this.apiService.getFromApi(`/reportTemplates`);
  }

  public getReportTemplate(reportTemplateID: number): Observable<ReportTemplateDto> {
    return this.apiService.getFromApi(`/reportTemplates/${reportTemplateID}`);
  }

  getReportTemplateModels(): Observable<any[]> {
    let route = `/reportTemplateModels`;
    return this.apiService.getFromApi(route);
  }

  
  public newReportTemplate(reportTemplateNewDto: ReportTemplateUpdateDto) {
    // return this.apiService.postToApi(`/reportTemplates/new`, reportTemplateNewDto);

     // we need to do it this way because the apiService.postToApi does a json.stringify, which won't work for input type="file"
     let formData = new FormData();
     formData.append("DisplayName", reportTemplateNewDto.DisplayName);
     if(reportTemplateNewDto.Description !== undefined){
      formData.append("Description", reportTemplateNewDto.Description);
     }
     formData.append("ReportTemplateModelID", reportTemplateNewDto.ReportTemplateModelID.toString());
     formData.append("FileResource", reportTemplateNewDto.FileResource);
    
     const apiHostName = environment.apiHostName;
     const route = `https://${apiHostName}/reportTemplates/new`;
     var result = this.httpClient.post<any>(
         route,
         formData
     );
     return result;
  }

  public updateReportTemplate(reportTemplateID: number, reportTemplateUpdateDto: ReportTemplateUpdateDto) {
     // we need to do it this way because the apiService.postToApi does a json.stringify, which won't work for input type="file"
     let formData = new FormData();
     formData.append("DisplayName", reportTemplateUpdateDto.DisplayName);
     if(reportTemplateUpdateDto.Description){
      formData.append("Description", reportTemplateUpdateDto.Description);
     }
     formData.append("ReportTemplateModelID", reportTemplateUpdateDto.ReportTemplateModelID.toString());
     if(reportTemplateUpdateDto.FileResource !== undefined){
      formData.append("FileResource", reportTemplateUpdateDto.FileResource);
     }
    
     const apiHostName = environment.apiHostName;
     const route = `https://${apiHostName}/reportTemplates/${reportTemplateID}`;
     var result = this.httpClient.put<any>(
         route,
         formData
     );
     return result;
  }

  public generateReport(generateReportsDto: GenerateReportsDto):  Observable<Blob> {
    const apiHostName = environment.apiHostName;
    const route = `https://${apiHostName}/reportTemplates/generateReports`;
    var result = this.httpClient.put(
        route,
        generateReportsDto,
        {
          // need to set the response type so it is not defauled to json
          responseType: 'blob'
        }
    );
    return result;
  }
}