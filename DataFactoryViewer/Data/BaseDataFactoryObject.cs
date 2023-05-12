using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;
using System.Reflection.Metadata.Ecma335;

namespace DataFactoryViewer.Data
{
    public abstract class BaseDataFactoryObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DataFactoryObjectType ObjectType { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public string Json { get; set; }
        public string AdfJson { get; set; }
        public string Bicep { get; set; }

        public BaseDataFactoryObject(SubResource resource, DataFactoryObjectType objectType, IAdfSerializer serializer, IJsonToBicepConverter converter)
        {
            Id = resource.Id;
            Name = resource.Name;
            ObjectType = objectType;
            
            // serialize whole object using our custom serializer
            Json = serializer.ToJson(resource);
            // serialize object with Adf serializer settings
            AdfJson = serializer.ToAdfJson(resource);
            // convert to bicep based on the Adf json
            Bicep = converter.Convert(resource.Name, ObjectType, AdfJson);
            
            Description = "";
            TypeName = "";
        }
    }
}
