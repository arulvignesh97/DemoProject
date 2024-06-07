// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpRequest, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';

@Injectable({
  providedIn: 'root'
})
export class MainspringMetricsService {
  public API: any;
  mainspring: any;

  constructor(private http: HttpClient, private config: AppConfig) {
    this.API = AppSettingsConfig.settings.API;
    this.mainspring = this.config.setting.mainspring;
  }
  getHttpParams(param): HttpParams {
    let httpparams = new HttpParams();
    Object.keys(param).forEach(x => {
      httpparams = httpparams.set(x, param[x]);
    });
    return httpparams;
  }
  MpsFilters(param: any): Observable<any> {
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.MpsFilters;
    return this.http.post(urlpath, param);
  }
  GetBaseMeasureFiltermainspringavailability(param: any): Observable<any> {
    const getParams = this.getHttpParams(param);
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.GetBaseMeasureFiltermainspringavailability;
    return this.http.get(urlpath, { params: getParams });
  }
  GetBaseMeasureProjectwiseSearchUserDefinedList(param): Observable<any> {
        const getParams = this.getHttpParams(param);
        const urlpath = this.API + this.mainspring.controllerName + this.mainspring.GetBaseMeasureProjectwiseSearchList;
        return this.http.get(urlpath, { params: getParams });
  }
  GetBaseMeasureLoadFactorProject(param: any): Observable<any> {
    const getParams = this.getHttpParams(param);
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.GetBaseMeasureLoadFactorProject;
    return this.http.get(urlpath, { params: getParams });
  }
  GetBaseMeasureProjectwiseSearchODCList(param: any): Observable<any> {
    const getParams = this.getHttpParams(param);
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.GetBaseMeasureProjectwiseSearchList;
    return this.http.get(urlpath, { params: getParams });
  }
  SaveBaseMeasureData(param: any): Observable<any> {
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.SaveBaseMeasureData;
    return this.http.post(urlpath, param);
  }
  GetTicketSummeryBaseMeasureODCList(param: any): Observable<any> {
    const getParams = this.getHttpParams(param);
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.GetTicketSummeryBaseMeasureODCList;
    return this.http.get(urlpath, { params: getParams });
  }
  SaveTicketSummaryBaseMeasureODC(param: any): Observable<any> {
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.SaveTicketSummaryBaseMeasureODC;
    return this.http.post(urlpath, param);
  }
  SaveLoadFactor(param: any): Observable<any> {
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.SaveLoadFactor;
    return this.http.post(urlpath, param);
  }
  GetBaseMeasureValueLoadFactor(param: any): Observable<any> {
    const getParams = this.getHttpParams(param);
    const urlpath = this.API + this.mainspring.controllerName + this.mainspring.GetBaseMeasureValueLoadFactor;
    return this.http.get(urlpath, { params: getParams });
  }
}
