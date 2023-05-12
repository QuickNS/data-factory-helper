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
    public class DatasetsController : ControllerBase
    {

        private readonly IDatasetHelper _datasetHelper;
        private readonly IAdfSerializer _adfSerializer;
        private readonly IJsonToBicepConverter _jsonToBicepConverter;

        public DatasetsController(IDatasetHelper datasetHelper, IAdfSerializer serializer, IJsonToBicepConverter converter)
        {
            _datasetHelper = datasetHelper;
            _adfSerializer = serializer;
            _jsonToBicepConverter = converter;
        }

        [HttpGet()]
        public async Task<IEnumerable<ListItemDto>> GetDatasetNames()
        {
            var datasets = await _datasetHelper.GetDatasetsAsync();
            return datasets.Select(ls => new ListItemDto { Name = ls.Name, TypeName = ls.Properties.GetType().Name });
        }


        [HttpGet("list")]
        public async Task<IEnumerable<DatasetResource>> ListDatasets()
        {
            var datasets = await _datasetHelper.GetDatasetsAsync();
            return datasets;
        }

        [HttpGet("{name}/json")]
        public async Task<DatasetResource> GetDatasetJson(string name)
        {
            var dataset = await _datasetHelper.GetDatasetAsync(name);
            return dataset;
            
        }

        [HttpGet("{name}/bicep")]
        public async Task<string> GetDatasetBicep(string name)
        {
            var dataset = await _datasetHelper.GetDatasetAsync(name);
            DatasetDto responseObject = new DatasetDto(dataset, _adfSerializer, _jsonToBicepConverter);
            return responseObject.Bicep;
        }
    }
}
