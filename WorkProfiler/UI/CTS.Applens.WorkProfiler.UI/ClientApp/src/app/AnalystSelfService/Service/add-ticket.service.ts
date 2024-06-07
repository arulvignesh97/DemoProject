// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseInformationModel, SelectedTicketModel } from 'src/app/common/models/BaseInformationModel';
import { AddTicketDetailsModel } from '../Models/AddTicketDetailsModel';

@Injectable({
  providedIn: 'root'
})
export class AddTicketService {
  ticketingModule: any;
  EffortTracking: any;
  apiURL: string;
  headers = {headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' }), withCredentials: true};
  constructor(private http: HttpClient, private config: AppConfig)
  {
    this.apiURL = AppSettingsConfig.settings.API;
    this.ticketingModule = this.config.setting.ticketingmodule;
    this.EffortTracking = this.config.setting.EffortTracking;
  }
  GetAddTicketPopup(param: BaseInformationModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetAddTicketPopup,
      param
     );
  }
  GetTicketIdByCustomerID(CustomerId: number): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetTicketIdByCustomerID +
      '/' + CustomerId, this.headers
     );
  }
  GetSelectedTicketDetails(param: SelectedTicketModel) : Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetSelectedTicketDetails,
      param
     );
  }
  GetDetailsByProjectID(ProjectID: number): Observable<any>{
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetDetailsByProjectID +
      '/' + ProjectID, this.headers
     );
  }
  GetDetailsAddTicket(param: BaseInformationModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.GetDetailsAddTicket,
      param
     );
  }
  AddTicketDetails(param: AddTicketDetailsModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.AddTicketDetails,
      param
     );
  }
  GetTicketInfoDetails(param: BaseInformationModel): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.EffortTracking.controllerName + this.EffortTracking.GetTicketInfoDetails,
      param
     );
  }
}
