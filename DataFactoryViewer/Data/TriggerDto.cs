using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryViewer.Data
{
    public class TriggerDto: BaseDataFactoryObject
    {
        public TriggerDto(TriggerResource resource, IAdfSerializer serializer)
            : base(resource, serializer)
        {
            Description = resource.Properties.Description;
            TypeName = resource.Properties.GetType().Name;
        }
    }
}
