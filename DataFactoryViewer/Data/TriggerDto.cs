using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryViewer.Data
{
    public class TriggerDto: BaseDataFactoryObject
    {
        public TriggerDto(TriggerResource resource, IAdfSerializer serializer, IJsonToBicepConverter converter)
            : base(resource, serializer, converter)
        {
            Description = resource.Properties.Description;
            TypeName = resource.Properties.GetType().Name;
        }
    }
}
