/**
 * Zybach
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
/* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { SensorAnomalySimpleDto } from '../model/sensor-anomaly-simple-dto';
import { SensorAnomalyUpsertDto } from '../model/sensor-anomaly-upsert-dto';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';
import { catchError } from 'rxjs/operators';
import { ApiService } from '../../services';


@Injectable({
  providedIn: 'root'
})
export class SensorAnomalyService {

    protected basePath = 'http://localhost';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration
    , private apiService: ApiService) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * 
     * 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public sensorAnomaliesGet(observe?: 'body', reportProgress?: boolean): Observable<Array<SensorAnomalySimpleDto>>;
    public sensorAnomaliesGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<SensorAnomalySimpleDto>>>;
    public sensorAnomaliesGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<SensorAnomalySimpleDto>>>;
    public sensorAnomaliesGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json',
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.get<Array<SensorAnomalySimpleDto>>(`${this.basePath}/sensorAnomalies`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        ).pipe(catchError((error: any) => { return this.apiService.handleError(error)}));
    }

    /**
     * 
     * 
     * @param sensorAnomalyUpsertDto 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public sensorAnomaliesNewPost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public sensorAnomaliesNewPost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public sensorAnomaliesNewPost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public sensorAnomaliesNewPost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json',
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.post<any>(`${this.basePath}/sensorAnomalies/new`,
            sensorAnomalyUpsertDto,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        ).pipe(catchError((error: any) => { return this.apiService.handleError(error)}));
    }

    /**
     * 
     * 
     * @param sensorAnomalyID 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public sensorAnomaliesSensorAnomalyIDDelete(sensorAnomalyID: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public sensorAnomaliesSensorAnomalyIDDelete(sensorAnomalyID: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public sensorAnomaliesSensorAnomalyIDDelete(sensorAnomalyID: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public sensorAnomaliesSensorAnomalyIDDelete(sensorAnomalyID: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (sensorAnomalyID === null || sensorAnomalyID === undefined) {
            throw new Error('Required parameter sensorAnomalyID was null or undefined when calling sensorAnomaliesSensorAnomalyIDDelete.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.delete<any>(`${this.basePath}/sensorAnomalies/${encodeURIComponent(String(sensorAnomalyID))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        ).pipe(catchError((error: any) => { return this.apiService.handleError(error)}));
    }

    /**
     * 
     * 
     * @param sensorAnomalyID 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public sensorAnomaliesSensorAnomalyIDGet(sensorAnomalyID: number, observe?: 'body', reportProgress?: boolean): Observable<SensorAnomalySimpleDto>;
    public sensorAnomaliesSensorAnomalyIDGet(sensorAnomalyID: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<SensorAnomalySimpleDto>>;
    public sensorAnomaliesSensorAnomalyIDGet(sensorAnomalyID: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<SensorAnomalySimpleDto>>;
    public sensorAnomaliesSensorAnomalyIDGet(sensorAnomalyID: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (sensorAnomalyID === null || sensorAnomalyID === undefined) {
            throw new Error('Required parameter sensorAnomalyID was null or undefined when calling sensorAnomaliesSensorAnomalyIDGet.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json',
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.get<SensorAnomalySimpleDto>(`${this.basePath}/sensorAnomalies/${encodeURIComponent(String(sensorAnomalyID))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        ).pipe(catchError((error: any) => { return this.apiService.handleError(error)}));
    }

    /**
     * 
     * 
     * @param sensorAnomalyUpsertDto 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public sensorAnomaliesUpdatePost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public sensorAnomaliesUpdatePost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public sensorAnomaliesUpdatePost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public sensorAnomaliesUpdatePost(sensorAnomalyUpsertDto?: SensorAnomalyUpsertDto, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json',
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.post<any>(`${this.basePath}/sensorAnomalies/update`,
            sensorAnomalyUpsertDto,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        ).pipe(catchError((error: any) => { return this.apiService.handleError(error)}));
    }

}