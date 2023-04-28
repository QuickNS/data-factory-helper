using DataFactoryHelper;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace DataFactoryViewer.Utils
{
    public class AdfSerializer : IAdfSerializer
    {
        private string[] ignoreProperties = new[] { "id", "etag", "runtimeState", "type" };

        private JsonSerializerSettings _serializerSettings;
        private ILogger<AdfSerializer> _logger;

        public AdfSerializer(ILogger<AdfSerializer> logger, IDataFactoryClient client)
        {
            _logger = logger;
            _serializerSettings =  new JsonSerializerSettings()
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
            if (o == null) { return String.Empty; }
            try
            {
                return SafeJsonConvert.SerializeObject(o, _serializerSettings);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return String.Empty;
            }
        }

    }
}
