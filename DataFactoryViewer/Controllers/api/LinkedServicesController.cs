using DataFactoryHelper;
using DataFactoryViewer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryViewer.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkedServicesController : ControllerBase
    {

        private readonly ILinkedServicesHelper _linkedServicesHelper;

        public LinkedServicesController(ILinkedServicesHelper linkedServicesHelper)
        {
            _linkedServicesHelper = linkedServicesHelper;
        }

        [HttpGet()]
        public async Task<IEnumerable<LinkedServiceResource>> ListLinkedServices()
        {
            var linkedServices = await _linkedServicesHelper.GetLinkedServicesAsync(); 
            return linkedServices;
        }

        [HttpGet("{name}")]
        public async Task<LinkedServiceResource> GetLinkedService(string name)
        {
            var linkedService = await _linkedServicesHelper.GetLinkedServiceAsync(name);
            return linkedService;
        }
    }
}
