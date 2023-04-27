using DataFactoryHelper;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;

namespace DataFactoryViewer.Utils
{
    public class AdfSerializer : IAdfSerializer
    {
        private string[] ignoreProperties = new[] { "id", "etag", "runtimeState", "type" };


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
                ContractResolver = new IgnorePropertiesResolver(ignoreProperties),
                Converters =
                {
                    new TransformationJsonConverter(),
                    new PolymorphicSerializeJsonConverter<LinkedService>("type"),
                    new PolymorphicSerializeJsonConverter<Trigger>("type"),
                    new PolymorphicSerializeJsonConverter<Dataset>("type"),
                    new PolymorphicSerializeJsonConverter<DatasetLocation>("type"),
                    new PolymorphicSerializeJsonConverter<PipelineReference>("type"),
                    new PolymorphicSerializeJsonConverter<LinkedServiceReference>("type")
                }
            };
        }

        public string ToJson(object o)
        {
            return SafeJsonConvert.SerializeObject(o, _serializerSettings);
        }


    }
}
