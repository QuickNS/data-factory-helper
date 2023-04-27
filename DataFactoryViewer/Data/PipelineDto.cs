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
    public class PipelineDto : BaseDataFactoryObject
    {
        public PipelineDto(PipelineResource resource, IAdfSerializer serializer)
            :base(resource, serializer)
        {
            Description = resource.Description;
            TypeName = "";
        }
    }
}
