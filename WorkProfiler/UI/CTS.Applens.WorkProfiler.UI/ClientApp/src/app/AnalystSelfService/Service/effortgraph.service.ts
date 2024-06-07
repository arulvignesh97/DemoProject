// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';

@Injectable({
  providedIn: 'root'
})
export class EffortgraphService {

  ticketingModule: any;
  apiURL: string;
  constructor(private http: HttpClient, private config: AppConfig){
  this.apiURL = AppSettingsConfig.settings.API;
  this.ticketingModule = this.config.setting.ticketingmodule;
}
GetEffortChartDetails(param:any): Observable<any> {
  return this.http.post(
    this.apiURL+
    this.ticketingModule.controllerName + this.ticketingModule.GetEffortChartDetails, 
    param
   );
}
}
