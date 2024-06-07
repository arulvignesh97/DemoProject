// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  ticketingModule: any;
  headerHeight: number = 0;
  footerHeight: number = 0;
  apiURL: string;
  public showConsoleErrors: boolean;
  public CopyrightURL: any;
  constructor(private http: HttpClient, private config: AppConfig)
  {
    this.apiURL = AppSettingsConfig.settings.API;
    this.ticketingModule = this.config.setting.ticketingmodule;
    this.showConsoleErrors = AppSettingsConfig.settings.ShowConsoleErrors;
    this.CopyrightURL = AppSettingsConfig.settings.CopyrightURL;
    if (!this.showConsoleErrors) {
      if (window) {
        window.console.log = function () { };
        window.console.error = function () { };
        window.console.warn = function () { };
      }
    }   
  }
  Getfooter(): Observable<any> {
    this.CopyrightURL = this.CopyrightURL + this.ticketingModule.GetFooterValue;
    let url = `${this.CopyrightURL}`;
    return this.http.get(url, { withCredentials: true, responseType: 'text' })
        .pipe(map(data => {
            return data;
        }));
  }
}
