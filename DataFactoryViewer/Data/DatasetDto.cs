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
    public class DatasetDto : BaseDataFactoryObject
    {
        public DatasetDto(DatasetResource resource, IAdfSerializer serializer, IJsonToBicepConverter converter)
            : base(resource, serializer, converter)
        {
            Description = resource.Properties.Description;
            TypeName = resource.Properties.GetType().Name;
        }
    }
}
