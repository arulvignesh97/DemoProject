// Copyright (c) Applens. All Rights Reserved.
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Constants } from 'src/app/common/constants/constants';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TicketEffortUploadServices {
ticketeffortupload: any;
  apiURL: string;

  constructor(private http: HttpClient, private config: AppConfig) {
  this.apiURL = AppSettingsConfig.settings.API;
  this.ticketeffortupload = this.config.setting.ticketeffortupload;
 }
 
  ProjectDetails(param: any): Observable<any> {
    return this.http.post(
    this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.ProjectDetails, 
    param
    );
  }

  ExportExcelClick(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/vnd.ms-excel' // for excel file
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };  
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.ExportExcelClick,
      param,requestOptions);
  }

  DownloadEffortUploadTemplate(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: Constants.ExcelType 
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.DownloadEffortUploadTemplate,
      param, requestOptions
    );
  }

  DownloadDebtTemplate(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/vnd.ms-excel' // for excel file
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };  
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.DownloadDebtTemplate,
      param ,requestOptions);
  }
  DownloadDebtUnClassifiedTickets(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: Constants.ExcelType
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.DownloadDebtUnClassifiedTickets,
      param, requestOptions);
  }

  UploadforDebtunclassifiedTicket(data: FormData): Observable<any> {

    const url = this.apiURL + this.ticketeffortupload.controllerName + this.ticketeffortupload.UploadforDebtUnclassifiedTicket;
    return this.http.post(url, data, { withCredentials: true })
      .pipe(map(data => {
        return data;
      }));
  }

  CheckITSM(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.CheckITSM,
      param
    );
  }

  GetUploadConfigDetails(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.GetUploadConfigDetails,
      param
    );
  }

  DownloadTicketTemplate(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/vnd.ms-excel' // for excel file
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };  
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.DownloadTicketTemplate,
      param,requestOptions);
  }

  Download(param: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: Constants.ExcelType 
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };
    return this.http.post(
      this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.Download,
      param, requestOptions
    );
  }

  TicketExcelUpload(data: FormData): Observable<any> {
    let apiType = this.ticketeffortupload.TicketExcelUpload;
    const url = this.apiURL + this.ticketeffortupload.controllerName + this.ticketeffortupload.TicketExcelUpload;
    return this.http.post(url, data, { withCredentials: true })
      .pipe(map(data => {
        return data;
      }));
  }

  TicketEffortUpload(data: FormData): Observable<any> {
    const url = this.apiURL + this.ticketeffortupload.controllerName + this.ticketeffortupload.TriggerEffortUpload;
    return this.http.post(url, data, { withCredentials: true })
      .pipe(map(data => {
        return data;
      }));
  }
  GetOnboardingPercentageDetails(param: any): Observable<any> {
    return this.http.post(
    this.apiURL +
      this.ticketeffortupload.controllerName + this.ticketeffortupload.GetOnboardingPercentageDetails, 
    param
    );
  }
}
