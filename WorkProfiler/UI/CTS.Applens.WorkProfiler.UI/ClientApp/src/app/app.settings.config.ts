// Copyright (c) Applens. All Rights Reserved.
import { IAppSettingsConfig } from './common/models/app.settings.config.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AppSettingsConfig {
  static settings: IAppSettingsConfig;
  //KeyCloak Added Code
  static isLogin = false;
  constructor(private http: HttpClient) { }
  load(): Promise<void> {
    const jsonFile = './assets/config/config.json';
    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFile).toPromise().then((response: IAppSettingsConfig) => {
        AppSettingsConfig.settings = ( response as IAppSettingsConfig);
        resolve();
      }).catch((response: any) => {
        reject(`Could not load file '${jsonFile}': ${JSON.stringify(response)}`);
      });
    });
  }
}
