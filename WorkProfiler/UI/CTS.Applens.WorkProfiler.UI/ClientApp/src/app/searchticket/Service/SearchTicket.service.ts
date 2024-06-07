// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Constants } from '../../common/constants/constants';

@Injectable({
  providedIn: 'root'
})
export class SearchTicketService {
  SearchTicket: any;
  apiURL: string;
  constructor(private http: HttpClient, private config: AppConfig)
  {
    this.apiURL = AppSettingsConfig.settings.API;
    this.SearchTicket = this.config.setting.SearchTicket;
  }
  GetSearchTickets(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.SearchTicket.controllerName + this.SearchTicket.GetSearchTickets,
      param
     );
  }
  GetHierarchyList(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.SearchTicket.controllerName + this.SearchTicket.GetHierarchyList,
      param
     );
  }
  GetTicketTypes(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.SearchTicket.controllerName + this.SearchTicket.GetTicketTypes,
      param
    );
  }
  GetTicketStatus(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.SearchTicket.controllerName + this.SearchTicket.GetTicketStatus,
      param
    );
  }
  DownloadSearchTicket(InputParms: any): Observable<any> {
    const headerOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: Constants.ExcelMacroType // for excel file
    });
    const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob' };
    return this.http.post
      (
        this.apiURL +
        this.SearchTicket.controllerName +
        this.SearchTicket.DownloadSearchTicket, InputParms, requestOptions
      );
  }
}
