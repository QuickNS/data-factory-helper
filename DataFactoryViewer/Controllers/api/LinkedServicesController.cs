using DataFactoryHelper;
using DataFactoryViewer.Data;
using DataFactoryViewer.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace DataFactoryViewer.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkedServicesController : ControllerBase
    {

        private readonly ILinkedServicesHelper _linkedServicesHelper;
        private readonly IAdfSerializer _adfSerializer;
        private readonly IJsonToBicepConverter _jsonToBicepConverter;

        public LinkedServicesController(ILinkedServicesHelper linkedServicesHelper, IAdfSerializer serializer, IJsonToBicepConverter converter)
        {
            _linkedServicesHelper = linkedServicesHelper;
            _adfSerializer = serializer;
            _jsonToBicepConverter = converter;
        }

        [HttpGet()]
        public async Task<IEnumerable<ListItemDto>> GetLinkedServiceNames()
        {
            var linkedServices = await _linkedServicesHelper.GetLinkedServicesAsync();
            return linkedServices.Select(ls => new ListItemDto { Name = ls.Name, TypeName = ls.Properties.GetType().Name });
        }


        [HttpGet("all")]
        public async Task<IEnumerable<LinkedServiceResource>> ListLinkedServices()
        {
            var linkedServices = await _linkedServicesHelper.GetLinkedServicesAsync();
            return linkedServices;
        }

        [HttpGet("{name}/json")]
        public async Task<LinkedServiceResource> GetLinkedServiceJson(string name)
        {
            var linkedService = await _linkedServicesHelper.GetLinkedServiceAsync(name);
            return linkedService;
        }

        [HttpGet("{name}/bicep")]
        public async Task<string> GetLinkedServiceBicep(string name)
        {
            var linkedService = await _linkedServicesHelper.GetLinkedServiceAsync(name);
            LinkedServiceDto responseObject = new LinkedServiceDto(linkedService, _adfSerializer, _jsonToBicepConverter);
            return responseObject.Bicep;
        }
    }
}
