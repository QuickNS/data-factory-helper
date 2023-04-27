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
    public class TriggersHelper : ITriggersHelper
    {
        private DataFactoryManagementClient _client;
        private string _resourceGroupName;
        private string _factoryName;

        public TriggersHelper(IDataFactoryClient client, string resourceGroupName, string factoryName)
        {
            _client = client.Initialize();
            _resourceGroupName = resourceGroupName;
            _factoryName = factoryName;
        }

        public async Task<List<TriggerResource>> GetTriggersAsync()
        {
            List<TriggerResource> triggers = new List<TriggerResource>();
            var page = await _client.Triggers.ListByFactoryAsync(_resourceGroupName, _factoryName);
            triggers.AddRange(page.AsEnumerable());
            while (page != null && !String.IsNullOrEmpty(page.NextPageLink))
            {
                page = await _client.Triggers.ListByFactoryNextAsync(page.NextPageLink);
                triggers.AddRange(page.AsEnumerable());
            }
            return triggers;
        }

        public async Task<TriggerResource> GetTrigger(string triggerName)
        {
            try
            {
                var trigger = await _client.Triggers.GetAsync(_resourceGroupName, _factoryName, triggerName);
                return trigger;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving trigger '{triggerName}': {ex.Message}");
                return null;
            }
        }
    }
}
