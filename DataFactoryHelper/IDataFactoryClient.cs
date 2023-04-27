using Microsoft.Azure.Management.DataFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactoryHelper
{
    public interface IDataFactoryClient
    {
        DataFactoryManagementClient Initialize();
    }
}
