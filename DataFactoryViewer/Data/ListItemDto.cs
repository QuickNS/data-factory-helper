using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataFactoryViewer.Data
{
    public class ListItemDto
    {
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string TypeName { get; set; }
    }
}
