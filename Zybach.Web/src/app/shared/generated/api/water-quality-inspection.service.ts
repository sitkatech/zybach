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

import { ClearinghouseWaterQualityInspectionDto } from '../model/clearinghouse-water-quality-inspection-dto';
import { WaterQualityInspectionSimpleDto } from '../model/water-quality-inspection-simple-dto';
import { WaterQualityInspectionTypeDto } from '../model/water-quality-inspection-type-dto';
import { WaterQualityInspectionUpsertDto } from '../model/water-quality-inspection-upsert-dto';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';
import { catchError } from 'rxjs/operators';
import { ApiService } from '../../services';


@Injectable({
  providedIn: 'root'
})
export class WaterQualityInspectionService {

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
    public clearinghouseWaterQualityInspectionsGet(observe?: 'body', reportProgress?: boolean): Observable<Array<ClearinghouseWaterQualityInspectionDto>>;
    public clearinghouseWaterQualityInspectionsGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<ClearinghouseWaterQualityInspectionDto>>>;
    public clearinghouseWaterQualityInspectionsGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<ClearinghouseWaterQualityInspectionDto>>>;
    public clearinghouseWaterQualityInspectionsGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.get<Array<ClearinghouseWaterQualityInspectionDto>>(`${this.basePath}/clearinghouseWaterQualityInspections`,
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
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public waterQualityInspectionTypesGet(observe?: 'body', reportProgress?: boolean): Observable<Array<WaterQualityInspectionTypeDto>>;
    public waterQualityInspectionTypesGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<WaterQualityInspectionTypeDto>>>;
    public waterQualityInspectionTypesGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<WaterQualityInspectionTypeDto>>>;
    public waterQualityInspectionTypesGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.get<Array<WaterQualityInspectionTypeDto>>(`${this.basePath}/waterQualityInspectionTypes`,
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
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public waterQualityInspectionsGet(observe?: 'body', reportProgress?: boolean): Observable<Array<WaterQualityInspectionSimpleDto>>;
    public waterQualityInspectionsGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<WaterQualityInspectionSimpleDto>>>;
    public waterQualityInspectionsGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<WaterQualityInspectionSimpleDto>>>;
    public waterQualityInspectionsGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.get<Array<WaterQualityInspectionSimpleDto>>(`${this.basePath}/waterQualityInspections`,
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
     * @param waterQualityInspectionUpsertDto 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public waterQualityInspectionsPost(waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe?: 'body', reportProgress?: boolean): Observable<WaterQualityInspectionSimpleDto>;
    public waterQualityInspectionsPost(waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<WaterQualityInspectionSimpleDto>>;
    public waterQualityInspectionsPost(waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<WaterQualityInspectionSimpleDto>>;
    public waterQualityInspectionsPost(waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


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
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json',
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.post<WaterQualityInspectionSimpleDto>(`${this.basePath}/waterQualityInspections`,
            waterQualityInspectionUpsertDto,
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
     * @param waterQualityInspectionID 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public waterQualityInspectionsWaterQualityInspectionIDDelete(waterQualityInspectionID: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public waterQualityInspectionsWaterQualityInspectionIDDelete(waterQualityInspectionID: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public waterQualityInspectionsWaterQualityInspectionIDDelete(waterQualityInspectionID: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public waterQualityInspectionsWaterQualityInspectionIDDelete(waterQualityInspectionID: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (waterQualityInspectionID === null || waterQualityInspectionID === undefined) {
            throw new Error('Required parameter waterQualityInspectionID was null or undefined when calling waterQualityInspectionsWaterQualityInspectionIDDelete.');
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

        return this.httpClient.delete<any>(`${this.basePath}/waterQualityInspections/${encodeURIComponent(String(waterQualityInspectionID))}`,
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
     * @param waterQualityInspectionID 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public waterQualityInspectionsWaterQualityInspectionIDGet(waterQualityInspectionID: number, observe?: 'body', reportProgress?: boolean): Observable<WaterQualityInspectionSimpleDto>;
    public waterQualityInspectionsWaterQualityInspectionIDGet(waterQualityInspectionID: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<WaterQualityInspectionSimpleDto>>;
    public waterQualityInspectionsWaterQualityInspectionIDGet(waterQualityInspectionID: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<WaterQualityInspectionSimpleDto>>;
    public waterQualityInspectionsWaterQualityInspectionIDGet(waterQualityInspectionID: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (waterQualityInspectionID === null || waterQualityInspectionID === undefined) {
            throw new Error('Required parameter waterQualityInspectionID was null or undefined when calling waterQualityInspectionsWaterQualityInspectionIDGet.');
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

        return this.httpClient.get<WaterQualityInspectionSimpleDto>(`${this.basePath}/waterQualityInspections/${encodeURIComponent(String(waterQualityInspectionID))}`,
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
     * @param waterQualityInspectionID 
     * @param waterQualityInspectionUpsertDto 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public waterQualityInspectionsWaterQualityInspectionIDPut(waterQualityInspectionID: number, waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public waterQualityInspectionsWaterQualityInspectionIDPut(waterQualityInspectionID: number, waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public waterQualityInspectionsWaterQualityInspectionIDPut(waterQualityInspectionID: number, waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public waterQualityInspectionsWaterQualityInspectionIDPut(waterQualityInspectionID: number, waterQualityInspectionUpsertDto?: WaterQualityInspectionUpsertDto, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (waterQualityInspectionID === null || waterQualityInspectionID === undefined) {
            throw new Error('Required parameter waterQualityInspectionID was null or undefined when calling waterQualityInspectionsWaterQualityInspectionIDPut.');
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
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json',
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.put<any>(`${this.basePath}/waterQualityInspections/${encodeURIComponent(String(waterQualityInspectionID))}`,
            waterQualityInspectionUpsertDto,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        ).pipe(catchError((error: any) => { return this.apiService.handleError(error)}));
    }

}