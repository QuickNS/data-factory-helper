using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryHelper
{
    public interface IDatasetHelper
    {
        Task<DatasetResource> GetDatasetAsync(string datasetName);
        Task<List<DatasetResource>> GetDatasetsAsync();
    }
}