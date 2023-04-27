using Microsoft.Azure.Management.DataFactory;
using Microsoft.Rest.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactoryHelper
{
    public static class Utils
    {
        public static string GetJson(DataFactoryManagementClient client, object o)
        {
            return SafeJsonConvert.SerializeObject(o, client.SerializationSettings);
        }
    }
}
