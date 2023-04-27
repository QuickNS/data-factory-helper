using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using System.Diagnostics;

namespace DataFactoryHelper
{
    public class LinkedServicesHelper : ILinkedServicesHelper
    {
        private DataFactoryManagementClient _client;
        private string _resourceGroupName;
        private string _factoryName;

        public LinkedServicesHelper(IDataFactoryClient client, string resourceGroupName, string factoryName)
        {
            _client = client.Initialize();
            _resourceGroupName = resourceGroupName;
            _factoryName = factoryName;
        }

        public async Task<List<LinkedServiceResource>> GetLinkedServicesAsync()
        {
            List<LinkedServiceResource> linkedServices = new List<LinkedServiceResource>();
            var page = await _client.LinkedServices.ListByFactoryAsync(_resourceGroupName, _factoryName);
            linkedServices.AddRange(page.AsEnumerable());
            while (page != null && !String.IsNullOrEmpty(page.NextPageLink))
            {
                page = await _client.LinkedServices.ListByFactoryNextAsync(page.NextPageLink);
                linkedServices.AddRange(page.AsEnumerable());
            }
            return linkedServices;
        }

        public async Task<LinkedServiceResource> GetLinkedServiceAsync(string linkedServiceName)
        {
            try
            {
                var linkedService = await _client.LinkedServices.GetAsync(_resourceGroupName, _factoryName, linkedServiceName);
                return linkedService;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving linked service '{linkedServiceName}': {ex.Message}");
                return null;
            }
        }
    }
}