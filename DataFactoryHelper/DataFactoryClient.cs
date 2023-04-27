using Microsoft.Azure.Management.DataFactory;
using Microsoft.Identity.Client;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactoryHelper
{
    public class DataFactoryClient : IDataFactoryClient
    {
        private string _tenantId;
        private string _clientId;
        private string _clientSecret;
        private string _subscriptionId;
        private DataFactoryManagementClient? _adfClient;

        public DataFactoryClient(string tenantId, string subscriptionId, string clientId, string clientSecret) { 
            _tenantId = tenantId;
            _clientId = clientId;   
            _clientSecret = clientSecret;
            _subscriptionId = subscriptionId;
        }
        public DataFactoryManagementClient Initialize()
        {
            if (_adfClient == null)
            {
                ServiceClientCredentials scc = GetCredentialsAsync().Result;
                Debug.WriteLine("Data Factory Client initialized");
                _adfClient = new DataFactoryManagementClient(scc) { SubscriptionId = _subscriptionId };
            }
            return _adfClient;
        }

        private async Task<ServiceClientCredentials> GetCredentialsAsync()
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_clientId)
                .WithAuthority($"https://login.microsoftonline.com/{_tenantId}")
                .WithClientSecret(_clientSecret)
                .WithLegacyCacheCompatibility(false)
                .WithCacheOptions(CacheOptions.EnableSharedCacheOptions)
                .Build();

            AuthenticationResult result = await app.AcquireTokenForClient(
                new string[] { "https://management.azure.com//.default" })
                .ExecuteAsync();

            ServiceClientCredentials cred = new TokenCredentials(result.AccessToken);
            return cred;
        }
    }
}
