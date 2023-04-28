using Newtonsoft.Json.Linq;
using System.Text;

namespace DataFactoryViewer.Utils
{
    public class JsonToBicepConverter : IJsonToBicepConverter
    {
        public string Convert(string json)
        {
            var jObj = JObject.Parse(json);

            // add parent property
            jObj.Add("parent", new JRaw("dataFactory"));

            // convert all json properties
            return $"{{\n{ConstructBicep(jObj.Root)}}}";
        }

        private string EscapeStringValue(JToken token)
        {
            return token.ToString().Replace("\\", "\\\\");
        }

        private string GetBicepForArray(JProperty prop, int indentLevel)
        {
            if (prop.Type != JTokenType.Array) { return String.Empty; }

            StringBuilder sb = new StringBuilder();
            var values = prop.Value.Values();
            if (values == null || values.Count() == 0)
            {
                sb = sb.AppendLine($"{prop.Name}: []");
            }
            else
            {
                sb = sb.AppendLine($"{prop.Name}: [");
                foreach (var v in values)
                {
                    sb = sb.Append(new string(' ', indentLevel + 2));
                    if (v.Type == JTokenType.String)
                    {
                        sb = sb.AppendLine($"'{v.ToString()}'");
                    }
                    else
                    {
                        sb = sb.AppendLine(v.ToString());
                    }
                }
            }
            sb = sb.Append(new string(' ', indentLevel));
            sb = sb.AppendLine("]");
            return sb.ToString();
        }

        private string GetBicepForString(JProperty prop)
        {
            return $"{prop.Name}: '{EscapeStringValue(prop.Value)}'\n";
        }

        private string GetBicepForBoolean(JProperty prop)
        {
            return $"{prop.Name}: {prop.Value.ToString().ToLower()}'\n";
        }

        private string GetBicepForObject(JProperty prop, int indentLevel)
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.Append($"{prop.Name}: {{\n{ConstructBicep(prop.Value, indentLevel + 2)}");
            sb = sb.AppendLine($"{new string(' ', indentLevel)}}}");
            return sb.ToString();
        }

        private string ConstructBicep(JToken token, int indentLevel = 2)
        {
            var propertyList = token.Select(x => (JProperty)x).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var prop in propertyList)
            {
                sb = sb.Append(new string(' ', indentLevel));
                if (prop.Value.Type == JTokenType.Object)
                {
                    sb = sb.Append(GetBicepForObject(prop, indentLevel));
                }
                else if (prop.Value.Type == JTokenType.Array)
                {
                    sb = sb.Append(GetBicepForArray(prop, indentLevel));
                }
                else if (prop.Value.Type == JTokenType.String)
                {
                    sb = sb.Append(GetBicepForString(prop));
                }
                else if (prop.Value.Type == JTokenType.Boolean)
                {
                    sb = sb.Append(GetBicepForBoolean(prop));
                }
                else
                {
                    sb = sb.AppendLine($"{prop.Name}: {prop.Value}");
                }
            }
            return sb.ToString();
        }
    }
}
