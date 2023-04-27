using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Azure.Management.DataFactory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactoryHelper
{
    public class DatasetHelper : IDatasetHelper
    {
        private DataFactoryManagementClient _client;
        private string _resourceGroupName;
        private string _factoryName;

        public DatasetHelper(IDataFactoryClient client, string resourceGroupName, string factoryName)
        {
            _client = client.Initialize();
            _resourceGroupName = resourceGroupName;
            _factoryName = factoryName;
        }

        public async Task<List<DatasetResource>> GetDatasetsAsync()
        {
            List<DatasetResource> datasets = new List<DatasetResource>();
            var page = await _client.Datasets.ListByFactoryAsync(_resourceGroupName, _factoryName);
            datasets.AddRange(page.AsEnumerable());
            while (page != null && !String.IsNullOrEmpty(page.NextPageLink))
            {
                page = await _client.Datasets.ListByFactoryNextAsync(page.NextPageLink);
                datasets.AddRange(page.AsEnumerable());
            }
            return datasets;
        }

        public async Task<DatasetResource> GetDatasetAsync(string datasetName)
        {
            try
            {
                var dataset = await _client.Datasets.GetAsync(_resourceGroupName, _factoryName, datasetName);
                return dataset;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving dataset '{datasetName}': {ex.Message}");
                return null;
            }
        }
    }
}
