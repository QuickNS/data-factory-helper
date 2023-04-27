using DataFactoryViewer.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DataFactoryViewer.Data
{
    public class DataFactoryConfigDto
    {
        [Display(Name = "Tenant Id")]
        public string TenantId { get; set; }
        [Display(Name = "Subscription Id")]
        public string SubscriptionId { get; set; }
        [Display(Name = "Client App Id")]
        public string ClientId { get; set; }
        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }
        [Display(Name = "Resource Group")]
        public string ResourceGroupName { get; set; }
        [Display(Name = "Data Factory")]
        public string FactoryName { get; set; }

        public DataFactoryConfigDto(DataFactoryConfig config)
        {
            TenantId = config.TenantId;
            SubscriptionId = config.SubscriptionId;
            ClientId = config.ClientId;
            ClientSecret = "*****************";
            ResourceGroupName = config.ResourceGroupName;
            FactoryName = config.FactoryName;
        }
    }
}
