using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryHelper
{
    public interface IDataflowsHelper
    {
        Task<List<DataFlowResource>> GetDataFlowsAsync();
        Task<DataFlowResource> GetDataFlowAsync(string linkedServiceName);
    }
}