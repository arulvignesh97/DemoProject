// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { AppConfig } from 'src/app/common/Config/config';
import { HttpClient,HttpParams,HttpHeaders } from '@angular/common/http';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ErroredTicketDetails } from '../Models/ErrorTicketsModels';

@Injectable({
  providedIn: 'root'
})
export class ErroredTicketsService {
  erroredTickets: any;
  apiURL: string;
  apiType: string = null;

  constructor(private http: HttpClient, private config: AppConfig) {
    this.apiURL = AppSettingsConfig.settings.API;
    this.erroredTickets = this.config.setting.erroredTickets;
   }
   getHttpParams(param): HttpParams {
    let httpparams = new HttpParams();
    Object.keys(param).forEach(x => {
      httpparams = httpparams.set(x, param[x]);
    });
    return httpparams;
  }
  getErrorLogTickets(params: any): Observable<any> {
    return this.http.post(
    this.apiURL +
    this.erroredTickets.controllerName + this.erroredTickets.GetErrorLogTickets,params
    );
  }
  getDebtFields(params: any): Observable<any> {
    return this.http.post(
    this.apiURL +
    this.erroredTickets.controllerName + this.erroredTickets.GetDebtFields,params
    );
  }
  getTicketDebtDetails(param: any): Observable<any> {
    const getParams = this.getHttpParams(param);
    return this.http.get(
     this.apiURL +
     this.erroredTickets.controllerName + this.erroredTickets.LoadErrorLogGrid,
     {params: getParams}
   );
 }
  getErrorLogTicketData(params: any): Observable<any> {
    return this.http.post(
    this.apiURL +
    this.erroredTickets.controllerName + this.erroredTickets.GetErrorLogTicketData,params
  );
  }
  saveErrorLogTicketData(erroredTicketDetails: ErroredTicketDetails){
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    const requestOptions = { headers: headerOptions };
    this.apiType = 'TicketingModule/SaveErrorLogTicketData';
    const url = `${this.apiURL}${this.apiType}`;
    return this.http.post(url, erroredTicketDetails, requestOptions).pipe(map((response) => {
      return response;
    }));
  }
}
