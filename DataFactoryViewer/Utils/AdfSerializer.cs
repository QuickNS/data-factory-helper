using DataFactoryHelper;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Extensions.Options;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace DataFactoryViewer.Utils
{
    public class AdfSerializer : IAdfSerializer
    {
        private JsonSerializerSettings _serializerSettings;
        private JsonSerializerSettings _adfSerializerSettings;
        private ILogger<AdfSerializer> _logger;

        private string[] propNamesToIgnore = new string[] { "id", "etag" };

        public AdfSerializer(ILogger<AdfSerializer> logger, IDataFactoryClient client)
        {
            _logger = logger;
            _adfSerializerSettings = client.Initialize().SerializationSettings;
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new AdfPropertiesResolver(propNamesToIgnore),
                Converters = JsonUtils.GetJsonConverters()
            };
        }

        public string ToJson(object o)
        {
            if (o == null) { return String.Empty; }
            try
            {
                return JsonConvert.SerializeObject(o, _serializerSettings);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return String.Empty;
            }
        }

        public string ToAdfJson(object o)
        {
            if (o == null) { return String.Empty; }
            try
            {
                return JsonConvert.SerializeObject(o, _adfSerializerSettings);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return String.Empty;
            }
        }

    }
}
