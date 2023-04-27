using DataFactoryHelper;
using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text.Json.Nodes;

namespace DataFactoryViewer.Data
{
    public class LinkedServiceDto: BaseDataFactoryObject
    {
        public LinkedServiceDto(LinkedServiceResource resource, IAdfSerializer serializer)
            : base(resource, serializer)
        {
            Description = resource.Properties.Description;
            TypeName = resource.Properties.GetType().Name;
            if(resource.Properties.Annotations != null && resource.Properties.Annotations.Count > 0)
            {
                Annotations = resource.Properties.Annotations.Select(a => a.ToString())
                    .ToList();
            }
        }
    }
}
