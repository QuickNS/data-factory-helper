using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataFactoryViewer.Utils
{
    public class AdfPropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<string> ignoreProps;
        public AdfPropertiesResolver(IEnumerable<string> propNamesToIgnore)
        {
            this.ignoreProps = new HashSet<string>(propNamesToIgnore);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var baseProps = base.CreateProperties(type, memberSerialization);
            var ordered = baseProps.OrderBy(p => p.PropertyName)
                .ToList();
            return ordered;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (this.ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }
            return property;
        }
    }
}
