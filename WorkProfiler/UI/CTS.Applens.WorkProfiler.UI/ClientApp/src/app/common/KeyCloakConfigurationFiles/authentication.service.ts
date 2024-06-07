/* © [2021] Cognizant. All rights reserved.  Cognizant Confidential and/or Trade Secret. 
NOTICE: This unpublished material is proprietary to Cognizant and its suppliers, if any. 
The methods, techniques and technical concepts herein are considered Cognizant confidential 
and/or trade secret information. 
This material may be covered by U.S. and/or foreign patents or patent applications. 
Use, distribution or copying, in whole or in part, is forbidden, 
except by express written permission of Cognizant. */

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, SecurityContext } from '@angular/core';
import { KeyCloakConstants } from '../KeyCloakConfigurationFiles/KeyCloakEum';
import { StorageHandlerService } from '../KeyCloakConfigurationFiles/storage-handler.service';
import { AppSettingsConfig } from '../../../app/app.settings.config';
import { Observable } from 'rxjs/internal/Observable';

@Injectable(
  { providedIn: 'root' }
)

export class AuthenticationHandler {
  private readonly authDuration: number = (AppSettingsConfig.settings.AuthenticationTimeoutDuration) ?
    (AppSettingsConfig.settings.AuthenticationTimeoutDuration) * KeyCloakConstants.Sixty * 
    KeyCloakConstants.Thousand : KeyCloakConstants.SixtyThousand;
  pdname;


  public userId = '0';
  errorCode: number;
  description: string;
  public observableResponse;
  private httpResponse;
  keycloakurl = '';
  constructor(private readonly http: HttpClient,
    private readonly storage: StorageHandlerService) {
    //Need to check
    // super(http,router,storage,loader,messageService);
    const token = this.storage.GetSessionData(KeyCloakConstants.AccessToken);
    this.SetTokenExpirationTimeout(token);
    this.keycloakurl = `${AppSettingsConfig.settings.Keycloak_BaseUrl}/${KeyCloakConstants.Realms}/${AppSettingsConfig.settings.Keycloak_Realm}/${KeyCloakConstants.TokenEndpoint}`;
  }
  private SetTokenExpirationTimeout(token) {
    let duration = this.authDuration;
    if (token && token?.duration) {
      duration = (token.duration - KeyCloakConstants.Five) * KeyCloakConstants.Sixty * KeyCloakConstants.Thousand;
    }
    const timeoutDef = setTimeout(() => {
      this.storage.RemoveSessions(KeyCloakConstants.AccessToken);
      clearTimeout(timeoutDef);
    }, duration);

  }

  get authToken() {
    const token = this.storage.GetSessionData(KeyCloakConstants.AccessToken);
    this.SetTokenExpirationTimeout(token);
    if (token && token != null) {
      return token;
    }
    else {
      return undefined;
    }
  }
  //Need to check
  // set authToken(value) {
  //   this.storage.SetSessionData("applens_authToken", value);
  //   //const duration = (value && value.duration) ? ((value.duration)*60*1000) : this.authDuration;        
  // }


  public AuthorizeUser() {
    return (AppSettingsConfig.isLogin && this.authToken);
  }

  //api for loging out from platform
  public LogOutUser(isHome = false) {
    this.storage.SetSessionData(KeyCloakConstants.AccessToken, undefined);
    this.storage.RemoveSessions('', true);
    if (isHome) {
      window.location.href = AppSettingsConfig.settings.HostUrl;
    }
    else {
      window.location.href = `${AppSettingsConfig.settings.HostUrl}#/home`;
    }
  }
  // Needed in future
  // async AuthenticateUser(loginData){        
  //     let data = {};
  //     data.UserName = loginData.username;
  //     data.Cred = loginData.cred;
  //     const url = "Authentication/Login";
  //     await super.post(url,data);        
  // }

  async KeyCloakAuthentication(loginData) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded',
        'Accept': '*/*'
      })
    };

    const pdname = 'password';

    const keylocakdata = new HttpParams({
      fromObject: {
        grant_type: pdname,
        client_id: AppSettingsConfig.settings.Client_id,
        client_secret: AppSettingsConfig.settings.Client_secret,
        username: loginData.username,
        [pdname]: loginData.cred.toString(),// Need to be checked String(this.SanitizeHTML(loginData.cred)),
        scope: 'openid'// need to check => this.scope
      }
    });

    await this.http.post(this.keycloakurl, keylocakdata, httpOptions)
      .toPromise()
      .then(
        (response) => {
          if (response && response[KeyCloakConstants.AccessToken]) {
            this.httpResponse = response;
            this.errorCode = KeyCloakConstants.TwoHundred;
            this.description = 'ok';
          }
        }
      )
      .catch(
        (error) => {
          this.description = 'fail';
        }
      )
      .finally(
        () => {
          this.OnFinalReturn();
        }
      );
  }

  OnFinalReturn() {
    let response;
    if (this.errorCode === KeyCloakConstants.TwoHundred && this.description && 
      this.description.toLowerCase() === 'ok') {
      response = this.httpResponse;
    }
    else if (this.errorCode === 0) {
      response = 'Service not available';
    }
    else {
      response = 'UnAuthorised';
    }

    this.observableResponse = new Observable((observer) => {
      observer.next(response);
      observer.complete();
    });
  }

  async RefereshToken(refereshToken = '') {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded',
        'Accept': '*/*'
      })
    };

    if (!refereshToken) {
      refereshToken = this.storage.GetSessionData(KeyCloakConstants.RefreshToken);
    }

    const keylocakdata = new HttpParams({
      fromObject: {
        grant_type: 'refresh_token',
        client_id: AppSettingsConfig.settings.Client_id,
        client_secret: AppSettingsConfig.settings.Client_secret,
        refresh_token: refereshToken
      }
    });

    // Need to check
    // await this.keycloakpost(keycloakurl, keylocakdata);
    await this.http.post(this.keycloakurl, keylocakdata, httpOptions)
      .toPromise()
      .then(
        (response) => {
          if (response && response[KeyCloakConstants.AccessToken]) {
            //Need to check the code
            // sessionStorage.setItem('applens_accesstoken', response.access_token);
            // this.storage.SetSessionData('applens_authTokenId', response.id_token);
            // this.storage.SetSessionData('applens_refreshToken', response.refresh_token);
            this.storage.SetSessionData(KeyCloakConstants.AccessToken, response[KeyCloakConstants.AccessToken]);
            AppSettingsConfig.isLogin = true;
          }
        }
      )
      .catch(
        (error) => {
          AppSettingsConfig.isLogin = false;
        }
      )
      .finally(
        () => {
          this.OnFinalReturn();
        }
      );
  }
} 
