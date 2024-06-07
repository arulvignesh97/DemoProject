// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpRequest, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';

@Injectable({
  providedIn: 'root'
})
export class ApproveunfreezeService {
  public API: any;
  approveunfreeze: any;
  continuouslearning: any;

  constructor(private http: HttpClient, private config: AppConfig) {
    this.API = AppSettingsConfig.settings.API;
    this.approveunfreeze = this.config.setting.approveunfreeze;
    this.continuouslearning = this.config.setting.continuouslearning;
  }

  GetAssignessOrDefaulters(param: any): Observable<any> {    
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetAssignessOrDefaulters;
    return this.http.post(urlpath, param);
  }

  GetTimeSheetDataDaily(param: any): Observable<any> {
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetTimeSheetDataDaily;
    return this.http.post(urlpath, param);
  }

  GetTimeSheetDataWeekly(param: any): Observable<any> {
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetTimeSheetDataWeekly;
    return this.http.post(urlpath, param);
  }

  GetAssignessDownload(param: any): Observable<any> {
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetAssignessDownload;
    return this.http.post(urlpath, param);
  }

  DownloadExcel(param: any): Observable<any> {
    let headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/vnd.ms-excel' // for excel file
    });
    let requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };  
    const urlpath = this.API + this.continuouslearning.controllerName + this.continuouslearning.DownloadExcel;
    return this.http.post(urlpath, param, requestOptions);
  }

  UpdateTimeSheetData(param: any,isDaily : boolean,userid:string): Observable<any> {
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.UpdateTimeSheetData+
    `/${isDaily}/${userid}`;
    return this.http.post(urlpath, param);
  }

  GetTicketDetailsPopUp(param: any): Observable<any> {
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetTicketDetailsPopUp;
    return this.http.post(urlpath, param);
  }

  GetTicketDetailsForDownload(param: any): Observable<any> {
    let headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/vnd.ms-excel' // for excel file
    });
    let requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetTicketDetailsForDownload;
    return this.http.post(urlpath, param, requestOptions);
  }

  GetCalendarData(param: any): Observable<any> {
    const urlpath = this.API + this.approveunfreeze.controllerName + this.approveunfreeze.GetCalendarData;
    return this.http.post(urlpath, param);
  }

}
