// Copyright (c) Applens. All Rights Reserved.
import { SpinnerService } from './common/services/spinner.service';
import { Component, ChangeDetectorRef, ViewRef, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { HeaderComponent } from './Layout/header/header.component';
declare var $: any;
import { HeaderService} from 'src/app/Layout/services/header.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
//KeyCloak related Imports
import { Idle, DEFAULT_INTERRUPTSOURCES } from '@ng-idle/core';
import { AuthService } from './common/KeyCloakConfigurationFiles/Azure-AD/auth.service';
import { AuthenticationHandler } from './common/KeyCloakConfigurationFiles/authentication.service';
import { OAuthService, OAuthSuccessEvent, OAuthErrorEvent } from 'angular-oauth2-oidc';
import { StorageHandlerService } from './common/KeyCloakConfigurationFiles/storage-handler.service';
import { AppSettingsConfig } from './app.settings.config';
import { KeyCloakConstants } from './common/KeyCloakConfigurationFiles/KeyCloakEum';
import { AzureAuthService } from '@library/msalauthfiles';
import { AppRoutes } from './common/constants/constants';
import { routes } from './app-routing.module';
// KeyCLoak End

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
    title = 'Work Profiler';
    displaySpinner = false;
    isLogin = false;
    isUserSet = false;
    roledropdown: any;
    username = '';
    role: any;
    sme: any;
    rhms: any = [];
    flag: any;
    userimage: any;
    userimage1: any;
    userimage_secured: any;
    array1: any = [];
    array2: any = [];
    outputList: any = [];
    selected_role: any;
    accountList: any = [];
    bu_for_operational: any = [];
    AccessDenied: any;
    sam: any;
    userId: any;
    //KeyCLoak Configuration Related Properties
appSettingsConfig =  AppSettingsConfig.settings;
isKeycloakEnabled = AppSettingsConfig.settings.KeyCloakEnabled;
IsAzureAdEnabled = AppSettingsConfig.settings.IsAzureADEnabled;
isUserLogin = false;
timed = false;
idleState = 'Not started.';
timedOut = false;
displayModal = false;
header = '';
claims!: unknown;
oidcLogin = false;
isTokenSuccessCalled = false;
displayPopup = true;
    constructor(private sanitizer: DomSanitizer,
                private activatedRoute: ActivatedRoute,
                public router: Router,
                private ref: ChangeDetectorRef,
                private spinner: SpinnerService,
                private headerService: HeaderService,
                private readonly idle:Idle,
                private readonly idpAuthService: AuthService,
                private readonly authHandler: AuthenticationHandler,
                private readonly oauthService: OAuthService,
                public readonly storage: StorageHandlerService,
                private azureauthService:AzureAuthService) {

                  // KeyCloak related Functions
                 if (this.isKeycloakEnabled) {
                  idle.setIdle(AppSettingsConfig.settings.IdleTimeOutInSec);
                  // sets a timeout period of 10 minutes. after 60 seconds of inactivity, the user will be considered timed out.
                  idle.setTimeout(AppSettingsConfig.settings.TimeOutCountDownPopUpStartsFromInSec);
                  idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);
                  idle.onTimeout.subscribe(() => {
                    this.idleState = '';
                    this.timedOut = true;
                    this.header = KeyCloakConstants.SessionExpired;
                    this.idle.stop();
                    this.idle.onTimeout.observers.length = 0;
                    this.idle.onIdleStart.observers.length = 0;
                    this.idle.onIdleEnd.observers.length = 0;
                    this.displayModal = true;
                  });
            
                  idle.onIdleStart.subscribe(() => {
                    this.displayModal = true;
                    this.idleState = '';
                    this.idle.clearInterrupts();
            
                  });
                  idle.onTimeoutWarning.subscribe((countdown) => {
                    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);
                    this.idleState = ` ${countdown}`;
                    if (countdown === 1) {
                      this.timed = true;
                      this.displayModal = true;
                    }
                  });
                  idle.onIdleEnd.subscribe(()=> {
                    this.timed = false;
                    this.displayModal = false;
                  });
                  this.reset();
                }
                // KeyCloak End
        setInterval(() => {
          if (this.ref !== null && this.ref !== undefined &&
              !(this.ref as ViewRef).destroyed) {
              this.ref.detectChanges();
          }
      }, 1000);
      }
    ngOnInit() {
      AppSettingsConfig.isLogin = this.idpAuthService.hasValidAccessToken();
      if (this.isKeycloakEnabled && !AppSettingsConfig.isLogin) {
        this.redirectOnCallback();
      }    
      else if (!this.isKeycloakEnabled && this.IsAzureAdEnabled) {
        const routeString = AppRoutes.timesheetEntry+','+AppRoutes.datadictionary+','+AppRoutes.ticketeffortupload+','
                            +AppRoutes.errorlog+','+AppRoutes.approveunfreeze+','+AppRoutes.debtreview+','
                            +AppRoutes.basemeasures+','+'searchticket';
        //MSAL guarded routes in comma seperated string
        this.azureauthService.login(routeString,routes);
         this.azureauthService.TokenRefresh.subscribe(x=>{
            if(x){
              AppSettingsConfig.isLogin = true;
              this.isUserLogin = true;
            }
         });
      }
      else if(AppSettingsConfig.isLogin){
        this.isUserLogin = true;
      }else{
        this.isUserLogin = true;
      }
      this.router.routeReuseStrategy.shouldReuseRoute = () => false;
       
      this.spinner.status.subscribe((val: boolean) => {
        setTimeout(() => {
          this.displaySpinner = val;
        }, 0);
      });
    }

    logout() {
      this.router.navigate(['logout']);
     
  }
// KeyCloak Integration Codes
reset() {
  this.idle.watch();
  this.timedOut = false;
  this.header = 'Session time out warning';
}
async redirectOnCallback() {
  this.oauthService.events.subscribe(event => {
    if (event instanceof OAuthErrorEvent) {
      AppSettingsConfig.isLogin = false;
    } else if (event instanceof OAuthSuccessEvent) {
      if (event?.type === KeyCloakConstants.TokenRecieved) {
        this.oauthService.loadUserProfile();
        this.claims = this.oauthService.getIdentityClaims();
        const isValidToken = this.oauthService.hasValidAccessToken();
        AppSettingsConfig.isLogin = isValidToken;
        this.isUserLogin = isValidToken;
      }
    }
    //Need to check code
    // else {
    //   AppSettingsConfig.isLogin = false;
    //     console.warn(event);
    // }
  });
}
stayLogIn() {
  this.displayModal = false;
  this.reset();
  if (this.storage.GetSessionData(KeyCloakConstants.AccessToken)) {
    this.storage.RemoveSessions(KeyCloakConstants.AccessToken);
    const refreshtkn = this.storage.GetSessionData(KeyCloakConstants.RefreshToken);
    this.authHandler.RefereshToken(refreshtkn);
  }
}

LogOutUser() {
  this.idpAuthService.logout();
  //Need to check code
  // else {
  //   this.authHandler.LogOutUser(this.route.url.includes("home"));
  // }
}
CloseSessionDialog(){
  if(this.timedOut){
    this.LogOutUser();
  }
}
LogInUser(){
   this.idpAuthService.login();
}
ReloadWindow(){
  window.location.reload();
}
// KeyCloak End



}