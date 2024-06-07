using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CTS.Applens.Framework;

namespace CTS.Applens.KeyVaultService
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "VaultOperationResult", Namespace = "http://schemas.datacontract.org/2004/07/KeyVaultService")]
    public partial class VaultOperationResult : object
    {

        private string ErrorMessageField;

        private byte[] ResultField;

        private int StatusCodeField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                this.ErrorMessageField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StatusCode
        {
            get
            {
                return this.StatusCodeField;
            }
            set
            {
                this.StatusCodeField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "AppAccess", Namespace = "http://schemas.datacontract.org/2004/07/KeyVaultService")]
    public partial class AppAccess : object
    {

        private int AppIdField;

        private string AppNameField;

        private byte[] HashValueField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AppId
        {
            get
            {
                return this.AppIdField;
            }
            set
            {
                this.AppIdField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppName
        {
            get
            {
                return this.AppNameField;
            }
            set
            {
                this.AppNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] HashValue
        {
            get
            {
                return this.HashValueField;
            }
            set
            {
                this.HashValueField = value;
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "KeyVaultService.IKeyVaultService")]
    public interface IKeyVaultService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/GetSecret", ReplyAction = "http://tempuri.org/IKeyVaultService/GetSecretResponse")]
        System.Threading.Tasks.Task<string> GetSecretAsync(int appId, string secretName);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/GetSecretProtected", ReplyAction = "http://tempuri.org/IKeyVaultService/GetSecretProtectedResponse")]
        System.Threading.Tasks.Task<string> GetSecretProtectedAsync(int appId, string secretName, string masterKeyId, string encryptionKeyId);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/FetchEncryptionKeyPrivateVault", ReplyAction = "http://tempuri.org/IKeyVaultService/FetchEncryptionKeyPrivateVaultResponse")]
        System.Threading.Tasks.Task<KeyVaultService.VaultOperationResult> FetchEncryptionKeyPrivateVaultAsync(int appId, string masterKeyId, string encryptionKeyId);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/GetAppList", ReplyAction = "http://tempuri.org/IKeyVaultService/GetAppListResponse")]
        System.Threading.Tasks.Task<KeyVaultService.AppAccess[]> GetAppListAsync();

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/FetchEncryptionKey", ReplyAction = "http://tempuri.org/IKeyVaultService/FetchEncryptionKeyResponse")]
        System.Threading.Tasks.Task<KeyVaultService.VaultOperationResult> FetchEncryptionKeyAsync(int appId, string masterKeyId, string encryptionKeyId);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/IsUserFromIntranet", ReplyAction = "http://tempuri.org/IKeyVaultService/IsUserFromIntranetResponse")]
        System.Threading.Tasks.Task<bool> IsUserFromIntranetAsync();

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IKeyVaultService/LogAudit", ReplyAction = "http://tempuri.org/IKeyVaultService/LogAuditResponse")]
        System.Threading.Tasks.Task LogAuditAsync(int operation, string keyName, string vaultId, int appId);
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class KeyVaultServiceClient : System.ServiceModel.ClientBase<KeyVaultService.IKeyVaultService>, KeyVaultService.IKeyVaultService
    {

        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public KeyVaultServiceClient() :
                base(KeyVaultServiceClient.GetDefaultBinding(), KeyVaultServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IKeyVaultService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public KeyVaultServiceClient(EndpointConfiguration endpointConfiguration) :
                base(KeyVaultServiceClient.GetBindingForEndpoint(endpointConfiguration), KeyVaultServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public KeyVaultServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
                base(KeyVaultServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public KeyVaultServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
                base(KeyVaultServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public KeyVaultServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public System.Threading.Tasks.Task<string> GetSecretAsync(int appId, string secretName)
        {
            return base.Channel.GetSecretAsync(appId, secretName);
        }

        public System.Threading.Tasks.Task<string> GetSecretProtectedAsync(int appId, string secretName, string masterKeyId, string encryptionKeyId)
        {
            return base.Channel.GetSecretProtectedAsync(appId, secretName, masterKeyId, encryptionKeyId);
        }

        public System.Threading.Tasks.Task<KeyVaultService.VaultOperationResult> FetchEncryptionKeyPrivateVaultAsync(int appId, string masterKeyId, string encryptionKeyId)
        {
            return base.Channel.FetchEncryptionKeyPrivateVaultAsync(appId, masterKeyId, encryptionKeyId);
        }

        public System.Threading.Tasks.Task<KeyVaultService.AppAccess[]> GetAppListAsync()
        {
            return base.Channel.GetAppListAsync();
        }

        public System.Threading.Tasks.Task<KeyVaultService.VaultOperationResult> FetchEncryptionKeyAsync(int appId, string masterKeyId, string encryptionKeyId)
        {
            return base.Channel.FetchEncryptionKeyAsync(appId, masterKeyId, encryptionKeyId);
        }

        public System.Threading.Tasks.Task<bool> IsUserFromIntranetAsync()
        {
            return base.Channel.IsUserFromIntranetAsync();
        }

        public System.Threading.Tasks.Task LogAuditAsync(int operation, string keyName, string vaultId, int appId)
        {
            return base.Channel.LogAuditAsync(operation, keyName, vaultId, appId);
        }

        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }

        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }

        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IKeyVaultService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                result.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IKeyVaultService))
            {
                return new System.ServiceModel.EndpointAddress("https://onecognizantcoresrvs.cognizant.com/2730/KeyVaultService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return KeyVaultServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IKeyVaultService);
        }

        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return KeyVaultServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IKeyVaultService);
        }

        public enum EndpointConfiguration
        {

            BasicHttpBinding_IKeyVaultService,
        }
    }

    public class MiddlewareKey
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
                int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);
        public static async Task<string> GetKeyVaultAsync(string userId, string domainName, string ConnectionString)
        {
            byte[] aesKey;
            byte[] dekRaw;
            string result = string.Empty;

            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_NETWORKCLEARTEXT = 8;

            string credential = GetValueForCredential(userId, ConnectionString);

            // Call LogonUser to obtain a handle to an access token.   
            SafeAccessTokenHandle safeAccessTokenHandle;
            bool isLogInSuccess = LogonUser(userId, domainName, credential, LOGON32_LOGON_NETWORKCLEARTEXT, LOGON32_PROVIDER_DEFAULT,
                                         out safeAccessTokenHandle);
            if (!isLogInSuccess)
            {
                int ret = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(ret);
            }

            await WindowsIdentity.RunImpersonated(safeAccessTokenHandle, async () =>
            {
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                basicHttpBinding.MaxReceivedMessageSize = 2147483647;
                basicHttpBinding.MaxBufferPoolSize = 524288;
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

                KeyVaultService.KeyVaultServiceClient client = new KeyVaultService.KeyVaultServiceClient(
                    basicHttpBinding,
                    new EndpointAddress("https://onecognizantcoresrvs.cognizant.com/2730/keyvaultservice.svc"));

                var res = await client.FetchEncryptionKeyAsync(800, "AVMDARTMasterKey", "AVMDARTKey3");
                dekRaw = res.Result;

                // validate if dekRaw is null, if you do not have the access to the keys, this will be null
                aesKey = new byte[32];
                Array.Copy(dekRaw, aesKey, 32);
                result = Convert.ToBase64String(aesKey);
            });
            return result;
        }
        public static string GetValueForCredential(string name, string ConnectionString)
        {
            string result = "";
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@Name", name);
            prms[1] = new SqlParameter("@Value", SqlDbType.NVarChar, 100);
            prms[1].Direction = ParameterDirection.Output;
            new DBHelper().ExecuteNonQuery("GetValueForCredential", prms, ConnectionString);
            result = Convert.ToString(prms[1].Value);
            return result;
        }
    }
}