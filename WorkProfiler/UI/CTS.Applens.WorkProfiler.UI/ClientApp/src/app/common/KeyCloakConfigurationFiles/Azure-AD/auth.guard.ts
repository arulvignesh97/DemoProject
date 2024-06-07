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
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { AppSettingsConfig } from '../../../../app/app.settings.config';
import { KeyCloakConstants } from '../KeyCloakEum';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor(private readonly authService: AuthService) {

  }
  isKeycloakEnabled = AppSettingsConfig.settings.KeyCloakEnabled;
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | UrlTree {
      const routedUrl = state.url;
      sessionStorage.setItem(KeyCloakConstants.CurrentRoutedUrl, routedUrl);
    if (this.authService.hasValidIdToken() || !this.isKeycloakEnabled) {
      return true;
    }
    this.authService.login();
    return false;
  }
}


