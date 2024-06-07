// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddworkitemService {
  ticketingModule: any;
  apiURL: string;
  constructor(private http: HttpClient, private config: AppConfig){
    this.apiURL = AppSettingsConfig.settings.API;
    this.ticketingModule = this.config.setting.ticketingmodule;
  }
  GetWorkItempopup(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetWorkItempopup,
      param
     );
  }
  GetDropDownValuesForWorkItem(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetDropDownValuesForWorkItem,
      param
     );
  }
  GetDropDownValuesSprint(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetDropDownValuesSprint,
      param
     );
  }
  CheckSprintName(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.CheckSprintName,
      param
     );
  }
  AddWorkItem(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.AddWorkItem,
      param
     );
  }
  CheckWorkItemId(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.CheckWorkItemId,
      param
     );
  }
  SaveSprintDetails(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.SaveSprintDetails,
      param
     );
  }
  GetSelectedTicketDetails(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetSelectedTicketDetails,
      param
     );
  }
}
