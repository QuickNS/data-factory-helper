using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataFactoryViewer.Utils
{
    public static class JsonUtils
    {
        public static List<JsonConverter> GetJsonConverters()
        {
            return new List<JsonConverter>
                {
                    new Iso8601TimeSpanConverter(),
                    new TransformationJsonConverter(),
                    new PolymorphicSerializeJsonConverter<SecretBase>("type"),
                    new PolymorphicSerializeJsonConverter<FactoryRepoConfiguration>("type"),
                    new PolymorphicSerializeJsonConverter<IntegrationRuntime>("type"),
                    new PolymorphicSerializeJsonConverter<IntegrationRuntimeStatus>("type"),
                    new PolymorphicSerializeJsonConverter<Credential>("type"),
                    new PolymorphicSerializeJsonConverter<WebLinkedServiceTypeProperties>("type"),
                    new PolymorphicSerializeJsonConverter<DatasetStorageFormat>("type"),
                    new PolymorphicSerializeJsonConverter<DependencyReference>("type"),
                    new PolymorphicSerializeJsonConverter<CompressionReadSettings>("type"),
                    new PolymorphicSerializeJsonConverter<ExportSettings>("type"),
                    new PolymorphicSerializeJsonConverter<ImportSettings>("type"),
                    new PolymorphicSerializeJsonConverter<LinkedIntegrationRuntimeType>("type"),
                    new PolymorphicSerializeJsonConverter<CustomSetupBase>("type"),
                    new PolymorphicSerializeJsonConverter<SsisObjectMetadata>("type"),
                    new PolymorphicSerializeJsonConverter<CopyTranslator>("type"),
                    new PolymorphicSerializeJsonConverter<LinkedService>("type"),
                    new PolymorphicSerializeJsonConverter<DataFlow>("type"),
                    new PolymorphicSerializeJsonConverter<Trigger>("type"),
                    new PolymorphicSerializeJsonConverter<Dataset>("type"),
                    new PolymorphicSerializeJsonConverter<DatasetLocation>("type"),
                    new PolymorphicSerializeJsonConverter<PipelineReference>("type"),
                    new PolymorphicSerializeJsonConverter<LinkedServiceReference>("type"),
                    new PolymorphicSerializeJsonConverter<FormatReadSettings>("type"),
                    new PolymorphicSerializeJsonConverter<FormatWriteSettings>("type"),
                    new PolymorphicSerializeJsonConverter<StoreReadSettings>("type"),
                    new PolymorphicSerializeJsonConverter<StoreWriteSettings>("type"),
                    new PolymorphicSerializeJsonConverter<CopySource>("type"),
                    new PolymorphicSerializeJsonConverter<CopySink>("type"),
                    new PolymorphicSerializeJsonConverter<Activity>("type")
                };
        }

        public static string GetJsonPropertyValueAsString(string propertyNameInDotNotation, JToken jToken)
        {
            var properties = propertyNameInDotNotation.Split('.');
            var currentNode = jToken;
            foreach (var property in properties)
            {
                var value = currentNode[property];
                if (value == null)
                    return String.Empty;
                else
                    currentNode = value;
            }
            return currentNode.ToString();
        }

        public static JArray GetJsonPropertyValueAsArray(string propertyNameInDotNotation, JToken jToken)
        {
            var properties = propertyNameInDotNotation.Split('.');
            var currentNode = jToken;
            foreach (var property in properties)
            {
                var value = currentNode[property];
                if (value == null)
                    return new JArray();
                else
                    currentNode = value;
            }
            return (JArray) currentNode;
        }

        public static void SetJsonPropertyValue(string propertyNameInDotNotation, JToken value, JToken jToken)
        {
            var properties = propertyNameInDotNotation.Split('.');
            var currentNode = jToken;
            foreach (var property in properties.SkipLast(1))
            {
                var node = currentNode[property];
                if (node == null)
                    return;
                else
                    currentNode = node;
            }
            currentNode[properties.Last()] = value;
            return;
        }
    }
}
