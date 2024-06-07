// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { AppConfig } from 'src/app/common/Config/config';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TicketAttributesService {
  ticketattributes: any;
  apiURL: string;
  headers = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' }), withCredentials: true };
  constructor(private http: HttpClient, private config: AppConfig) {
    this.apiURL = AppSettingsConfig.settings.API;
    this.ticketattributes = this.config.setting.ticketAttributes;
  }
  getAttributeDetails(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.GetAttributeDetails, params
    );
  }
  popupAttribute(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.PopupAttribute, params
    );
  }
  ticketAttributeDetails(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.TicketAttributeDetails, params
    );
  }
  getResolutionPriority(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.GetResolutionPriority, params
    );
  }
  getCauseCodeResolutionCode(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.CauseCodeResolutionCode, params
    );
  }
  insertAttributeDetails(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.InsertAttributeDetails, params
    );
  }
  checkIsGracePeriodMet(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.CheckIsGracePeriodMet, params
    );
  }

  getAlgoKey(projectId: number, supportTypeId: number): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.GetAlgoKeyAndColumn + '/'
      + projectId + '/' + supportTypeId, this.headers);
  }

  newAlgoClassification(params: any): Observable<any> {
    return this.http.post(
      this.apiURL +
      this.ticketattributes.controllerName + this.ticketattributes.NewAlgoClassification, params, this.headers);
  }
}
