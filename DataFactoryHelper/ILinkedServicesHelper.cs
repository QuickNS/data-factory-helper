using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactoryHelper
{
    public interface ILinkedServicesHelper
    {
        Task<List<LinkedServiceResource>> GetLinkedServicesAsync();
        Task<LinkedServiceResource> GetLinkedServiceAsync(string linkedServiceName);
    }
}
