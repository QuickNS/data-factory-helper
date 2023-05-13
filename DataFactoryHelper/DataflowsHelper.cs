using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using System.Diagnostics;

namespace DataFactoryHelper
{
    public class DataflowsHelper : IDataflowsHelper
    {
        private DataFactoryManagementClient _client;
        private string _resourceGroupName;
        private string _factoryName;

        public DataflowsHelper(IDataFactoryClient client, string resourceGroupName, string factoryName)
        {
            _client = client.Initialize();
            _resourceGroupName = resourceGroupName;
            _factoryName = factoryName;
        }

        public async Task<List<DataFlowResource>> GetDataFlowsAsync()
        {
            List<DataFlowResource> dataflows = new List<DataFlowResource>();
            var page = await _client.DataFlows.ListByFactoryAsync(_resourceGroupName, _factoryName);
            dataflows.AddRange(page.AsEnumerable());
            while (page != null && !String.IsNullOrEmpty(page.NextPageLink))
            {
                page = await _client.DataFlows.ListByFactoryNextAsync(page.NextPageLink);
                dataflows.AddRange(page.AsEnumerable());
            }
            return dataflows;
        }

        public async Task<DataFlowResource> GetDataFlowAsync(string dataflowName)
        {
            try
            {
                var dataflow = await _client.DataFlows.GetAsync(_resourceGroupName, _factoryName, dataflowName);
                return dataflow;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving dataflow '{dataflowName}': {ex.Message}");
                return null;
            }
        }
    }
}