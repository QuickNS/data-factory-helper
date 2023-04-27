using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using System.Diagnostics;

namespace DataFactoryHelper
{
    public class PipelinesHelper : IPipelinesHelper
    {
        private DataFactoryManagementClient _client;
        private string _resourceGroupName;
        private string _factoryName;

        public PipelinesHelper(IDataFactoryClient client, string resourceGroupName, string factoryName)
        {
            _client = client.Initialize();
            _resourceGroupName = resourceGroupName;
            _factoryName = factoryName;
        }

        public async Task<List<PipelineResource>> GetPipelinesAsync()
        {
            List<PipelineResource> pipelines = new List<PipelineResource>();
            var page = await _client.Pipelines.ListByFactoryAsync(_resourceGroupName, _factoryName);
            pipelines.AddRange(page.AsEnumerable());
            while (page != null && !String.IsNullOrEmpty(page.NextPageLink))
            {
                page = await _client.Pipelines.ListByFactoryNextAsync(page.NextPageLink);
                pipelines.AddRange(page.AsEnumerable());
            }
            return pipelines;
        }

        public async Task<PipelineResource> GetPipeline(string pipelineName)
        {
            try
            {
                var pipeline = await _client.Pipelines.GetAsync(_resourceGroupName, _factoryName, pipelineName);
                return pipeline;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving pipeline '{pipelineName}': {ex.Message}");
                return null;
            }
        }
    }
}