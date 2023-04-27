using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryHelper
{
    public interface ITriggersHelper
    {
        Task<TriggerResource> GetTrigger(string triggerName);
        Task<List<TriggerResource>> GetTriggersAsync();
    }
}