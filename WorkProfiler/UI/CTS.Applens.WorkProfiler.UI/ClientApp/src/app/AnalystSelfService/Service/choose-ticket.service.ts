// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { BaseInformationModel } from 'src/app/common/models/BaseInformationModel';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AppConfig } from 'src/app/common/Config/config';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { ChooseTicket } from '../Models/ChooseTicket';

@Injectable({
  providedIn: 'root'
})
export class ChooseTicketService {
  ticketingModule: any;
  EffortTracking: any;
  apiURL: string;
  headers = {headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' }), withCredentials: true};

  constructor(private http: HttpClient, private config: AppConfig){
    this.apiURL = AppSettingsConfig.settings.API;
    this.ticketingModule = this.config.setting.ticketingmodule;
    this.EffortTracking = this.config.setting.EffortTracking;
  }
  ChooseUnAssignedTicket(param: BaseInformationModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.ChooseUnAssignedTicket,
      param
     );
  }
  GetTowerDetailsByProjectID(param: BaseInformationModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetTowerDetailsByProjectID,
      param
     );
  }
  GetApplicationDetailsByProject(ProjectID): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetApplicationDetailsByProject +
      '/' +ProjectID, this.headers
     );
  }
  GetTicketStatusByProject(ProjectID: number): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetTicketStatusByProject +
      '/' + ProjectID, this.headers
     );
  }
  GetAssignmentGroupByProject(param: BaseInformationModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetAssignmentGroupByProject,
      param
     );
  }
  ChooseTicketDetails(param: ChooseTicket): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.EffortTracking.controllerName + this.EffortTracking.ChooseTicketDetails,
      param
     );
  }
}
