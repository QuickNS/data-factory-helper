using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;
using System.Reflection.Metadata.Ecma335;

namespace DataFactoryViewer.Data
{
    public abstract class BaseDataFactoryObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string TypeName { get; set; }
        public string Json { get; set; }
        public string Bicep { get; set; }

        public BaseDataFactoryObject(SubResource resource, IAdfSerializer serializer, IJsonToBicepConverter converter)
        {
            Id = resource.Id;
            Name = resource.Name;
            Json = serializer.ToJson(resource);
            Description = "";
            TypeName = "";
            Bicep = converter.Convert(Json);
        }
    }
}
