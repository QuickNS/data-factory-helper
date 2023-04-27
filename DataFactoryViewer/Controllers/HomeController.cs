using DataFactoryHelper;
using DataFactoryViewer.Data;
using DataFactoryViewer.Models;
using DataFactoryViewer.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DataFactoryViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ILinkedServicesHelper _linkedServicesHelper;
        private readonly IDatasetHelper _dataSetHelper;
        private readonly IPipelinesHelper _pipelinelinesHelper;
        private readonly IAdfSerializer _serializer;
        
        private readonly DataFactoryConfig _dataFactoryConfig;
        

        public HomeController(IOptions<DataFactoryConfig> config, IAdfSerializer serializer, IPipelinesHelper pipelinesHelper, IDatasetHelper datasetHelper, ILinkedServicesHelper linkedServicesHelper, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataFactoryConfig = config.Value;
            _linkedServicesHelper = linkedServicesHelper;
            _dataSetHelper = datasetHelper;
            _pipelinelinesHelper = pipelinesHelper;
            _serializer = serializer;
        }

        public IActionResult Index()
        {
            var model = new FactoryDataDto();

            // data factory config
            model.DataFactoryConfig = new DataFactoryConfigDto(_dataFactoryConfig);

            // linked services
            var linkedServices = _linkedServicesHelper.GetLinkedServicesAsync().Result;
            
            foreach (var linkedService in linkedServices)
            {
                model.LinkedServices.Add(new LinkedServiceDto(linkedService, _serializer));
            }

            //datasets
            var datasets = _dataSetHelper.GetDatasetsAsync().Result;
            foreach (var dataset in datasets)
            {
                model.Datasets.Add(new DatasetDto(dataset, _serializer));
            }

            //pipelines
            var pipelines = _pipelinelinesHelper.GetPipelinesAsync().Result;
            foreach (var pipeline in pipelines)
            {
                model.Pipelines.Add(new PipelineDto(pipeline, _serializer));
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}