import { Injectable } from '@angular/core';
import { ApiService } from '.';
import { Observable } from 'rxjs';
import { ReportTemplateDto } from '../models/generated/report-template-dto';
import { ReportTemplateNewDto } from '../models/report-template-new-dto';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

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

  
  public newReportTemplate(reportTemplateNewDto: ReportTemplateNewDto) {
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

  public updateReportTemplate(reportTemplateID: number, reportTemplateNewDto: ReportTemplateNewDto) {
     // we need to do it this way because the apiService.postToApi does a json.stringify, which won't work for input type="file"
     let formData = new FormData();
     formData.append("DisplayName", reportTemplateNewDto.DisplayName);
     if(reportTemplateNewDto.Description !== undefined){
      formData.append("Description", reportTemplateNewDto.Description);
     }
     formData.append("ReportTemplateModelID", reportTemplateNewDto.ReportTemplateModelID.toString());
     if(reportTemplateNewDto.FileResource !== undefined){
      formData.append("FileResource", reportTemplateNewDto.FileResource);
     }
    
     const apiHostName = environment.apiHostName;
     const route = `https://${apiHostName}/reportTemplates/${reportTemplateID}`;
     var result = this.httpClient.put<any>(
         route,
         formData
     );
     return result;
  }

  // public updateReportTemplate(reportTemplate: ReportTemplateDto): Observable<ReportTemplateDto> {
  //   return this.apiService.putToApi(`reportTemplates/${reportTemplate.ReportTemplateID}`, reportTemplate);
  // }
}