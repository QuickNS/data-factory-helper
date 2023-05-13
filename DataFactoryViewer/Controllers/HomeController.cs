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
        private readonly IDataflowsHelper _dataflowsHelper;
        private readonly ITriggersHelper _triggersHelper;
        private readonly IAdfSerializer _serializer;
        private readonly IJsonToBicepConverter _bicepConverter;
        private readonly DataFactoryConfig _dataFactoryConfig;
        

        public HomeController(
            IOptions<DataFactoryConfig> config,
            IAdfSerializer serializer,
            IJsonToBicepConverter converter,
            IPipelinesHelper pipelinesHelper,
            IDataflowsHelper dataflowsHelper,
            ITriggersHelper triggersHelper,
            IDatasetHelper datasetHelper,
            ILinkedServicesHelper linkedServicesHelper,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataFactoryConfig = config.Value;
            _linkedServicesHelper = linkedServicesHelper;
            _dataSetHelper = datasetHelper;
            _triggersHelper = triggersHelper;
            _pipelinelinesHelper = pipelinesHelper;
            _dataflowsHelper = dataflowsHelper;
            _serializer = serializer;
            _bicepConverter = converter;
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
                model.LinkedServices.Add(new LinkedServiceDto(linkedService, _serializer, _bicepConverter));
            }

            //datasets
            var datasets = _dataSetHelper.GetDatasetsAsync().Result;
            foreach (var dataset in datasets)
            {
                model.Datasets.Add(new DatasetDto(dataset, _serializer, _bicepConverter));
            }

            //pipelines
            var pipelines = _pipelinelinesHelper.GetPipelinesAsync().Result;
            foreach (var pipeline in pipelines)
            {
                model.Pipelines.Add(new PipelineDto(pipeline, _serializer, _bicepConverter));
            }

            //dataflows
            var dataflows = _dataflowsHelper.GetDataFlowsAsync().Result;
            foreach (var dataflow in dataflows)
            {
                model.Dataflows.Add(new DataflowDto(dataflow, _serializer, _bicepConverter));
            }

            //triggers
            var triggers = _triggersHelper.GetTriggersAsync().Result;
            foreach (var trigger in triggers)
            {
                model.Triggers.Add(new TriggerDto(trigger, _serializer, _bicepConverter));
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