import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CapturePostData } from '../shared/generated/model/capture-post-data';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class PrintService {

  private webappUrl: string;
  constructor(private apiService: ApiService, private http: HttpClient) { 
    if(environment.dev){
      this.webappUrl = `http://host.docker.internal:8713`;
    } else {
      this.webappUrl = window.location.origin;
    }
  }
  
  public htmlToPdf(html: string): Observable<any> {
    const data = {
      'html': html,
      'cssUrls': [
        `${this.webappUrl}/print-module.css`
      ]
    } as CapturePostData;
    
    return this.apiService.postToApi(`print/pdf`, data, 'arraybuffer');
  }

  public htmlToImage(html: string): Observable<any> {
    const data = {
      'html': html,
      'cssSelector': "#wellChart",
      'cssUrls': [
        `${this.webappUrl}/print-module.css`
      ]
    } as CapturePostData;
    
    return this.apiService.postToApi(`print/image`, data, 'arraybuffer');
  }
}