using DataFactoryViewer.Data;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Extensions.Options;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFactoryViewer.Utils
{
    public class JsonToBicepConverter : IJsonToBicepConverter
    {
        private const string apiVersion = "2018-06-01";
        private string factoryName;

        public JsonToBicepConverter(IOptions<DataFactoryConfig> config)
        {
            factoryName = config.Value.FactoryName;
        }

        public string Convert(string resourceName, DataFactoryObjectType objectType, string json)
        {
            var jObj = JObject.Parse(json);
            // add parent property
            jObj.AddFirst(new JProperty("parent", new JRaw("dataFactory")));

            // add name property
            jObj.AddFirst(new JProperty("name", GetNamePropertyValue(objectType)));
            
            // convert to JToken
            var jToken = JToken.FromObject(jObj);

            var sb = new StringBuilder();
            // add parameters
            sb.Append(GetParameters(resourceName, objectType, jToken));

            // add factoryReference
            sb.Append(GetFactoryReference());

            // add resource header
            sb.Append(GetHeader(objectType));

            // convert the json object to Bicep template
           
            sb.Append($"{{\n{GetBicep(jToken, 2)}}}");
            return sb.ToString();
        }

        public string GetBicep(JToken token, int indentLevel = 0)
        {
            var propertyList = token.Select(x => (JProperty)x).ToList();

            string bicepCode = "";

            foreach (var property in propertyList)
            {
                bicepCode += new string(' ', indentLevel);
                if (property.Value.Type == JTokenType.Object)
                {
                    // nested object
                    bicepCode += $"{property.Name}: {{\n";
                    bicepCode += GetBicep(property.Value, indentLevel + 2);
                    bicepCode += new string(' ', indentLevel);
                    bicepCode += "}\n";
                }
                else if (property.Value.Type == JTokenType.Array)
                {
                    // array
                    JArray jsonArray = (JArray)property.Value;

                    if (jsonArray.Count == 0)
                    {
                        bicepCode += $"{property.Name}: []\n";
                    }
                    else
                    {
                        bicepCode += $"{property.Name}: [\n";
                        foreach (var item in jsonArray)
                        {
                            if (item.Type == JTokenType.Object)
                            {
                                bicepCode += new string(' ', indentLevel + 2) + "{\n";
                                bicepCode += GetBicep(item, indentLevel + 4);
                                bicepCode += new string(' ', indentLevel + 2) + "},\n";
                            }
                            else
                            {
                                bicepCode += new string(' ', indentLevel + 2);
                                bicepCode += $"{item},\n";
                            }
                        }

                        bicepCode = bicepCode.TrimEnd(',', '\n') + "\n" + new string(' ', indentLevel) + "]\n";
                    }

                }
                else if (property.Value.Type == JTokenType.String)
                {
                    bicepCode += $"{property.Name}: '{EscapeStringValue(property.Value.Value<string>())}'\n";
                }
                else if (property.Value.Type == JTokenType.Boolean)
                {
                    bicepCode += $"{property.Name}: {property.Value.Value<string>().ToLower()}\n";
                }
                else
                {
                    bicepCode += $"{property.Name}: {property.Value.Value<string>()}\n";
                }
            }

            return bicepCode;
        }

        private string EscapeStringValue(JToken token)
        {
            return token.ToString().Replace("\\", "\\\\").Replace("'", "\\'");
        }

        private string GetParameters(string resourceName, DataFactoryObjectType ObjectType, JToken jToken)
        {
            var sb = new StringBuilder();
            sb.Append($"param factoryName string = '{factoryName}' \n");
            switch (ObjectType)
            {
                case DataFactoryObjectType.Dataset:
                    {
                        sb.Append($"param datasetName string = '{resourceName}'\n");
                        var paramValue = JsonUtils.GetJsonPropertyValueAsString("properties.linkedServiceName.referenceName", jToken);
                        if (!String.IsNullOrEmpty(paramValue))
                        {
                            JsonUtils.SetJsonPropertyValue("properties.linkedServiceName.referenceName", new JRaw("linkedServiceName"), jToken);
                            sb.Append($"param linkedServiceName string = '{paramValue}'\n");
                        }
                        break;
                    }
                case DataFactoryObjectType.LinkedService:
                    {
                        sb.Append($"param linkedServiceName string = '{resourceName}'\n");
                        break;
                    }
                case DataFactoryObjectType.Trigger:
                    {
                        sb.Append($"param triggerName string = '{resourceName}'\n");
                        var pipelines = JsonUtils.GetJsonPropertyValueAsArray("properties.pipelines", jToken);
                        int pipelineCounter = 1;
                        foreach (var pipeline in pipelines)
                        {
                            var paramValue = JsonUtils.GetJsonPropertyValueAsString("pipelineReference.referenceName", pipeline);
                            JsonUtils.SetJsonPropertyValue("pipelineReference.referenceName", new JRaw($"pipelineName{pipelineCounter}"), pipeline);
                            sb.Append($"param pipelineName{pipelineCounter} string = '{paramValue}'\n");
                        }

                        break;
                    }
                case DataFactoryObjectType.Pipeline:
                    {
                        sb.Append($"param pipelineName string = '{resourceName}'\n");
                        break;
                    }
                case DataFactoryObjectType.Dataflow:
                    {
                        sb.Append($"param dataflowName string = '{resourceName}'\n");
                        break;
                    }
                default: break;
            }
            return sb.ToString();
        }

        private JToken GetNamePropertyValue(DataFactoryObjectType objectType)
        {
            switch (objectType)
            {
                case DataFactoryObjectType.Dataset:
                    return new JRaw("datasetName");
                case DataFactoryObjectType.LinkedService:
                    return new JRaw("linkedServiceName");
                case DataFactoryObjectType.Pipeline:
                    return new JRaw("pipelineName");
                case DataFactoryObjectType.Trigger:
                    return new JRaw("triggerName");
                case DataFactoryObjectType.Dataflow:
                    return new JRaw("dataflowName");
                default: return new JRaw("");
            }
        }

        private string GetHeader(DataFactoryObjectType objectType)
        {
            switch (objectType)
            {
                case DataFactoryObjectType.Dataset:
                    return $"\nresource dataset 'Microsoft.DataFactory/factories/datasets@{apiVersion}' = ";
                case DataFactoryObjectType.LinkedService:
                    return $"\nresource linkedService 'Microsoft.DataFactory/factories/linkedservices@{apiVersion}' = ";
                case DataFactoryObjectType.Pipeline:
                    return $"\nresource pipeline 'Microsoft.DataFactory/factories/pipelines@{apiVersion}' = ";
                case DataFactoryObjectType.Trigger:
                    return $"\nresource trigger 'Microsoft.DataFactory/factories/triggers@{apiVersion}' = ";
                case DataFactoryObjectType.Dataflow:
                    return $"\nresource trigger 'Microsoft.DataFactory/factories/dataflows@{apiVersion}' = ";
                default: return String.Empty;
            }
        }

        private string GetFactoryReference()
        {
            return $"\nresource dataFactory 'Microsoft.DataFactory/factories@{apiVersion}' existing = {{\n  name: factoryName\n}}\n";
        }

    }
}
