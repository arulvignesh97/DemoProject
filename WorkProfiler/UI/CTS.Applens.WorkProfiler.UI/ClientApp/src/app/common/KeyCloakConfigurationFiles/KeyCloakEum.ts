/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
*Copyright [2018] – [2021] Cognizant. All rights reserved.
*NOTICE: This unpublished material is proprietary to Cognizant and
*its suppliers, if any. The methods, techniques and technical
  concepts herein are considered Cognizant confidential and/or trade secret information.
 
*This material may be covered by U.S. and/or foreign patents or patent applications.
*Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/
//Keycloak
export enum KeyCloakConstants {
  TokenEndpoint = 'protocol/openid-connect/token',
  UserinfoEndpoint = 'protocol/openid-connect/userinfo',
  RevocationEndpoint = 'protocol/openid-connect/revoke',
  RedirectUri = 'callback',
  PostLogoutRedirectUri = '',
  OidcAuthProvider = 'oidc',
  LDAPAuthProvider = 'ldap',
  SessionExpired = 'Session expired',

  //KeyClock Keys
  TokenRecieved = 'token_received',
  ApplensUserID = 'applens_userId',
  UserPrincipalName = 'user_principal_name',
  AccessToken = 'access_token',
  RefreshToken = 'refresh_token',
  Bearer = 'Bearer',
  Realms = 'realms',
  SessionChanged = 'session_changed',
  Thousand = 1000,
  Five = 5,
  Sixty = 60,
  SixtyThousand = 60000,
  TwoHundred = 200,
  CurrentRoutedUrl = 'CurrentRoutedUrl'
}


