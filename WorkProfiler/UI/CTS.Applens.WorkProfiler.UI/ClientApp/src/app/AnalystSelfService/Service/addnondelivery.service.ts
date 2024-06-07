// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddnondeliveryService {
  ticketingModule: any;
  apiURL: string;
  constructor(private http: HttpClient, private config: AppConfig){
    this.apiURL = AppSettingsConfig.settings.API;
    this.ticketingModule = this.config.setting.ticketingmodule;
  }
  GetNonTicketPopup(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetNonTicketPopup,
      param
     );
  }
  SaveNonTicketDetails(param): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.SaveNonTicketDetails,
      param
     );
  }
}
