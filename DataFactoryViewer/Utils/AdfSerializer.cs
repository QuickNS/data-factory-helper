using DataFactoryHelper;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;

namespace DataFactoryViewer.Utils
{
    public class AdfSerializer : IAdfSerializer
    {
       
        private JsonSerializerSettings _serializerSettings;
        public AdfSerializer(IDataFactoryClient client)
        {
            _serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new IgnorePropertiesResolver(new[] { "id", "etag", "type" }),
                Converters =
                {
                    new TransformationJsonConverter(),
                    new PolymorphicSerializeJsonConverter<LinkedService>("type")
                }
            };
        }

        public string ToJson(object o)
        {
            return SafeJsonConvert.SerializeObject(o, _serializerSettings);
        }
    }
}
