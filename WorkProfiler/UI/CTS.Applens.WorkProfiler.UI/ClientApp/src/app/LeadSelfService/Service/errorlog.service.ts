// Copyright (c) Applens. All Rights Reserved.
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Constants } from 'src/app/common/constants/constants';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorLogServices {
errorlog: any;
apiURL: string;

constructor(private http: HttpClient, private config: AppConfig) {
  this.apiURL = AppSettingsConfig.settings.API;
  this.errorlog = this.config.setting.errorlog;
 }
 
  ProjectDetails(param: any): Observable<any> {
    return this.http.post(
    this.apiURL +
      this.errorlog.controllerName + this.errorlog.ProjectDetails, 
    param
    );
  }

  ErrorLog(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.errorlog.controllerName + this.errorlog.ErrorLog,
      param
    );
  }

  ExportExcelClick(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.errorlog.controllerName + this.errorlog.ExportExcelClick,
      param
    );
  }

  Download(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: Constants.ExcelType
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };
    return this.http.post(
      this.apiURL +
      this.errorlog.controllerName + this.errorlog.Download,
      param, requestOptions
    );
  }

  GetConfigDetails(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.errorlog.controllerName + this.errorlog.GetConfigDetails,
      param
    );
  }

}
