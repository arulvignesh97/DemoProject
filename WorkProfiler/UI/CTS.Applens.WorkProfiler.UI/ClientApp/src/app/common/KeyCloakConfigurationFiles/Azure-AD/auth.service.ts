/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
*Copyright [2018] – [2021] Cognizant. All rights reserved.
*NOTICE: This unpublished material is proprietary to Cognizant and
*its suppliers, if any. The methods, techniques and technical
  concepts herein are considered Cognizant confidential and/or trade secret information.
 
*This material may be covered by U.S. and/or foreign patents or patent applications.
*Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/

import { Injectable } from '@angular/core';
import { OAuthService, AuthConfig, OAuthErrorEvent, OAuthSuccessEvent, NullValidationHandler }
  from 'angular-oauth2-oidc';
import { filter } from 'rxjs/operators';
import { KeyCloakConstants } from '../KeyCloakEum';
import { StorageHandlerService } from '../storage-handler.service';
import { AppSettingsConfig } from '../../../../app/app.settings.config';
import { IAppSettingsConfig } from '../../models/app.settings.config.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  authConfig: AuthConfig;
  jsonData: IAppSettingsConfig;
  issuerUrl: string;

  constructor(private readonly oauthService: OAuthService,
    public readonly storageService: StorageHandlerService) {
    if (AppSettingsConfig?.settings?.KeyCloakEnabled) {
      /**
      * The OpenID-Connect configuration using the Authorization Code flow
      */

      this.jsonData = AppSettingsConfig.settings;
      this.issuerUrl = `${this.jsonData.Keycloak_BaseUrl}/${KeyCloakConstants.Realms}/${this.jsonData.Keycloak_Realm}`;
      this.authConfig = {
        issuer: this.issuerUrl,
        tokenEndpoint: `${this.issuerUrl}/${KeyCloakConstants.TokenEndpoint}`,
        userinfoEndpoint: `${this.issuerUrl}/${KeyCloakConstants.UserinfoEndpoint}`,
        revocationEndpoint: `${this.issuerUrl}/${KeyCloakConstants.RevocationEndpoint}`,
        redirectUri: this.jsonData.HostUrl,
        postLogoutRedirectUri: this.jsonData.PostLogoutRedirectUri,
        clientId: this.jsonData.Client_id,
        dummyClientSecret: this.jsonData.Client_secret,
        scope: this.jsonData.Scope,
        responseType: this.jsonData.ResponseType,
        sessionChecksEnabled: this.jsonData.SessionChecksEnabled,
        redirectUriAsPostLogoutRedirectUriFallback: this.jsonData.RedirectUriAsPostLogoutRedirectUriFallback,
        clearHashAfterLogin: this.jsonData.ClearHashAfterLogin
      };
      this.configure();
      // For SSO logout
      this.oauthService.events.pipe(filter(e => e.type === KeyCloakConstants.SessionChanged)).subscribe(e => {
        this.logout();
      });
    }
  }

  /**
   * Extract the roles from the realm_access claim within the Keycloak generated access token (JWT)
   */
  public getClaims(): string[] {
    const accessToken: string = this.oauthService.getAccessToken();
    const tokens: string[] = accessToken.split('.');
    const claims = JSON.parse(atob(tokens[1]));
    return claims.realm_access.roles;
  }

  /**
   * Extracts the OpenID Connect clientId from the Keycloak generated access token (JWT)
   */
  public getClientId(): string {
    const claims = this.getJwtAsObject();
    return claims['azp'];
  }

  /**
   * Extracts the JWT Issuer from the Keycloak generated access token
   */
  public getIssuer(): string {
    const claims = this.getJwtAsObject();
    return claims['iss'];
  }

  /**
   * Will kick-off the OpenID Connect Authorization Code flow (Based on the configured authConfig#issuer)
   */
  public login(): void {
    this.oauthService.initLoginFlow();
  }

  /**
   * Will execute a logout operation by re-directing to Keycloaks logout endpoint and successively to
   * to a configured logout path (Configured above in authConfig#postLogoutRedirectUri)
   */
  public logout(): void {

    this.storageService.RemoveSessions('', true);
    this.oauthService.logOut();
  }

  /**
  * Determines if the current user has a valid id token
  * Returns true if an IdToken exists within the session storage, false otherwise
  */
  public hasValidIdToken(): boolean {
    return this.oauthService.hasValidIdToken();
  }
  /**
   * Determines if the current user has a valid id token
   * Returns true if an AccessToken exists within the session storage, false otherwise
   */
  public hasValidAccessToken(): boolean {
    return this.oauthService.hasValidAccessToken();
  }
  /**
   * Configures the Angular OpenID Connect client
   */
  private configure(): void {
    this.oauthService.configure(this.authConfig);
    this.oauthService.setupAutomaticSilentRefresh();
    this.oauthService.tokenValidationHandler = new NullValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin().then((data) => {
      if (!this.hasValidAccessToken()) {
        this.login();
      }
    });
  }

  /**
   * Helper method to extract the claims from the body component of the signed access token
   */
  private getJwtAsObject(): object {
    const accessToken: string = this.oauthService.getAccessToken();
    const tokens: string[] = accessToken.split('.');
    return JSON.parse(atob(tokens[1]));
  }
}


