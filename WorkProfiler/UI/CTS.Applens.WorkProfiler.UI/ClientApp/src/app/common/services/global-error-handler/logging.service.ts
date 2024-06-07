// Copyright (c) Applens. All Rights Reserved.
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { AppSettingsConfig } from './../../../app.settings.config';
import { AppConfig } from '../../Config/config';
import { ClientError } from '../../models/client-error';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  public jsondata: any;
  public learningAPIUrl: string;
  public clientError: ClientError;

  constructor(private readonly http: HttpClient, private readonly config: AppConfig) {
    this.jsondata = AppSettingsConfig.settings;
    this.learningAPIUrl = this.jsondata.API;
  }
  logError(message: string, stack: string) {
    // Send errors to be saved here
    if (this.jsondata.ShowConsoleErrors) {
      console.log('LoggingService: ' + message);
    }
    else {
      this.clientError = new ClientError();
      this.clientError.ErrorMessage = message;
      this.clientError.StackTrace = stack;

      this.http.post(
        this.learningAPIUrl +
        'Logging/LogError', this.clientError, this.config.setting['httpoptions']);
    }
  }
}
