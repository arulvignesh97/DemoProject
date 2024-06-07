// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { Associate } from './../../Models/header';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  ticketingModule: any;
  datadictionary: any;
  userDetails: any;
  apiURL: string;
  getCurrentUserInfo:any;
  apiType: string = null;
  AssociateDetails: Associate;
  masterDataEmitter = new BehaviorSubject<any>(null);
  languageDataEmitter = new BehaviorSubject<any>(null);
  constructor(private http: HttpClient, private config: AppConfig) 
{
  this.apiURL = AppSettingsConfig.settings.API;
  this.ticketingModule = this.config.setting.ticketingmodule;
  this.datadictionary = this.config.setting.dataDictionary;
  this.userDetails = this.config.setting.userDetails;
  this.getCurrentUserInfo=this.config.setting.getCurrentUserInfo;
}

GetLanguageDetails(): Observable<any> {
      return this.http.get(
        this.apiURL+
        this.ticketingModule.controllerName + this.ticketingModule.GetLanguageDetails
       );
    }
    GetCacheDetails(): Observable<any> {
          return this.http.get(
            this.apiURL+
            this.userDetails.controllerName + this.userDetails.GetHiddenDetails
           );
        }
  SaveLanguageDetails(param:any): Observable<any> {
        return this.http.post(
          this.apiURL+
          this.ticketingModule.controllerName + this.ticketingModule.SaveLanguageDetails,
          param
         );
      }
      GetHiddenFields(param:any): Observable<any> {
            return this.http.post(
              this.apiURL+
              this.ticketingModule.controllerName + this.ticketingModule.GetHiddenFields,
              param
             );
          }
      GetProjectLeadDetails(param:any): Observable<any> {
          return this.http.post(
              this.apiURL+
              this.ticketingModule.controllerName + this.ticketingModule.GetProjectLeadDetails,
              param
        );
      }

      GetProjectDetailsforDefaultLanding(param:any): Observable<any> {
        return this.http.post(
            this.apiURL+
            this.ticketingModule.controllerName + this.ticketingModule.GetProjectDetailsforDefaultLanding,
            param
      );
    }
    GetDefaultLandingPageDetails(param:any): Observable<any> {
      return this.http.post(
          this.apiURL+
          this.ticketingModule.controllerName + this.ticketingModule.GetDefaultLandingPageDetails,
          param
    );
  }

  SaveDefaultLandingPageDetails(param:any): Observable<any> {
    return this.http.post(
        this.apiURL+
        this.ticketingModule.controllerName + this.ticketingModule.SaveDefaultLandingPageDetails,
        param
  );
}

GetUserProfilePicture(): Observable<Associate> {
  return this.http.get(
    this.apiURL +
    this.ticketingModule.controllerName + this.ticketingModule.GetUserProfilePicture).pipe(map((response:Associate)=>{
    this.AssociateDetails = response;
    return response;
    })); 
}

GetMyAssociateURL(): Observable<any> { 
  return this.http.get(
    this.apiURL+
    this.ticketingModule.controllerName + this.ticketingModule.GetMyAssociateURL    
   );
}
GetHomePage(): Observable<any> { 
  return this.http.get(
    this.apiURL+
    this.ticketingModule.controllerName + this.ticketingModule.GetHomePage    
   );
}
GetNavMenuUrl(): Observable<any> { 
  return this.http.get(
    this.apiURL+
    this.ticketingModule.controllerName + this.ticketingModule.GetNavMenuUrl    
   );
}
GetCustomerwiseDefaultPage(param:any): Observable<any> {
  return this.http.post(
      this.apiURL+
      this.ticketingModule.controllerName + this.ticketingModule.GetCustomerwiseDefaultPage,
      param
);
}

GetTicketRoles(param:any): Observable<any> {
  return this.http.post(
      this.apiURL+
      this.datadictionary.controllerName + this.datadictionary.GetTicketRoles,
      param
);
}
GetRoles(param:any): Observable<any> {
  return this.http.post(
      this.apiURL+
      this.datadictionary.controllerName + this.datadictionary.GetRoles,
      param
);
}

}
