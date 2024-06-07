// Copyright (c) Applens. All Rights Reserved.
import { AppConfig } from './../common/Config/config';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { Injectable } from '@angular/core';
import { HttpClient,HttpParams,HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AnalystselfserviceService {

  analystSelfServiceAttributes: any;
  apiURL: string;
  headers = {headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' }), withCredentials: true};
  constructor(private http: HttpClient, private config: AppConfig) {
    this.apiURL = AppSettingsConfig.settings.API;
    this.analystSelfServiceAttributes = this.config.setting.analystSelfService;
  }
  getHttpParams(param): HttpParams {
    let httpparams = new HttpParams();
    Object.keys(param).forEach(x => {
      httpparams = httpparams.set(x, param[x]);
    });
    return httpparams;
  }
  getProjectName(param: any): Observable<any> {
      const getParams = this.getHttpParams(param);
      return this.http.get(
       this.apiURL +
       this.analystSelfServiceAttributes.controllerName + this.analystSelfServiceAttributes.GetProjectName,
       {params: getParams}
     );
   }

  getCurrentTimeofTimeZones(param: any): Observable<any> {
      const getParams = this.getHttpParams(param);
      return this.http.get(
      this.apiURL +
      this.analystSelfServiceAttributes.controllerName + this.analystSelfServiceAttributes.GetCurrentTimeofTimeZones,
      {params: getParams}
    );
  }
  getAssigneNameByProjectId(params: any): Observable<any> {
    const getParams = this.getHttpParams(params);
    return this.http.get(
      this.apiURL +
      this.analystSelfServiceAttributes.controllerName + this.analystSelfServiceAttributes.GetAssigneNameByProjectID,
      {params: getParams}
    );
  }
}
