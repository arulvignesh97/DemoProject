using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.Framework
{
    class Constants
    {
        public static readonly string dataImage = "data:image/";
        public static readonly string base64 = "; base64,";

    }
    public static class KeyCloakConstants
    {
        public static readonly string KeyCloakEnabled = "KeyCloakEnabled";
        public static readonly string KeyCloakOidc = "oidc";
        public static readonly string KeyCloakSaml = "saml";
        public static readonly string KeyCloakldap = "ldap";
        public static readonly string KeyCloakOidcAuthProvider = "KeyCloakOidc:AuthProvider";
        public static readonly string KeyCloakOidcEmailDomain = "KeyCloakOidc:EmailDomain";
        public static readonly string UserPrincipalName = "user_principal_name";
        public static readonly string PreferredUserName = "preferred_username";
        public static readonly string GivenName = "givenname";
        public static readonly string SurName = "surname";
        public static readonly string KeyCloakOidcSplitter = "KeyCloakOidc:Splitter";
        public static readonly string PrincipalName = "KeyCloakOidc:PrincipalName";
        public static readonly string KeyCloakAuthority = "Authority";
        public static readonly string GrantType = "GrantType";

        public static readonly string Realm = "Realm";
        public static readonly string ClientId = "ClientId";
        public static readonly string Scope = "scope";
        public static readonly string ClientSecret = "ClientSecret";
        public static readonly string GraphAPI = "GraphAPI";
        public static readonly string ImageType = "data:image/png;base64,";
        public static readonly string IdentityProvider = "/broker/keycloak-oidc/token";
    }
}
