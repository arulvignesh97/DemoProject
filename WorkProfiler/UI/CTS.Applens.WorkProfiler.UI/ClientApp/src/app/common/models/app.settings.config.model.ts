// Copyright (c) Applens. All Rights Reserved.
export interface IAppSettingsConfig {
  Production: boolean;
  API: string;
  CopyrightURL: string;
  IsMacroTemplateForDebtEnabled: boolean;
  IsBenchMarkApplicable: boolean;
  ADMApplicableforCustomer: boolean;
  DailyFreezeDay: number;
  ShowConsoleErrors: boolean;
  IsDebtUnClassifiedApplicableforSAAS: boolean;
  //common header
  HeaderPath: string,
  FooterPath: string,
  CommonCss: string,
  AppStoreURL : string,
  //common header end
  IsAzureADEnabled:boolean;
  //KeyCloakFields
  KeyCloakEnabled :boolean;
  Keycloak_BaseUrl:string;
  Keycloak_Realm: string;
  Keycloak_AuthProvider: string;
  Client_id: string;
  Client_secret: string;
  AuthenticationTimeoutDuration:number;
  HostUrl:string;
  IdleTimeOutInSec:number;
  TimeOutCountDownPopUpStartsFromInSec:number;
  EmailDomain: string; 
  Scope: string,
  ResponseType: string,
  SessionChecksEnabled: boolean,
  RedirectUriAsPostLogoutRedirectUriFallback:boolean,
  ClearHashAfterLogin: boolean
  PostLogoutRedirectUri: string;
  PrincipalName: string;
  Splitter: string;
}
