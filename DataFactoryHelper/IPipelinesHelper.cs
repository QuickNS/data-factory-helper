using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryHelper
{
    public interface IPipelinesHelper
    {
        Task<PipelineResource> GetPipeline(string pipelineName);
        Task<List<PipelineResource>> GetPipelinesAsync();
    }
}