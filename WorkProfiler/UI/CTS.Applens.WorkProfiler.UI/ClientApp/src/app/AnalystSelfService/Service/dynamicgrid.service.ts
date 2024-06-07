// Copyright (c) Applens. All Rights Reserved.
import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';

@Injectable({
  providedIn: 'root'
})

export class DynamicgridService {
  ticketingModule: any;
  apiURL: string;
  checkGraceperiodEmitter = new EventEmitter<any>();
  DatechangeEmitter = new BehaviorSubject<any>(null);

  constructor(private http: HttpClient, private config: AppConfig){
  this.apiURL = AppSettingsConfig.settings.API;
  this.ticketingModule = this.config.setting.ticketingmodule;
}

MandatoryHours(param:any): Observable<any> {
      return this.http.post(
        this.apiURL+
        this.ticketingModule.controllerName + this.ticketingModule.GetMandatoryHours, 
        param
       );
}
  Getticketdetail(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.Getticketdetail,
      param
    );
  }
  updateWorkItemServiceandStatus(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.UpdateWorkItemServiceandStatus,
      param
    );
  }
  updateIsAttributeByTicketId(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.UpdateIsAttributeByTicketId,
      param
    );
  }
  SaveGridData(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.SaveData,
      param
    );
  }
  DeleteGridData(param: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketingModule.controllerName + this.ticketingModule.Deletegriddata,
      param
    );
  }
  getCountErroredTickets(params: any): Observable<any> {
    return this.http.post(
    this.apiURL +
    this.ticketingModule.controllerName + this.ticketingModule.GetCountErroredTickets,params
    );
  }
}
