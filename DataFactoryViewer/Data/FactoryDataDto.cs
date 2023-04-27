namespace DataFactoryViewer.Data
{
    public class FactoryDataDto
    {
        public List<LinkedServiceDto> LinkedServices { get; set; } = new List<LinkedServiceDto>();
        public List<DatasetDto> Datasets { get; set; } = new List<DatasetDto>();
        public List<PipelineDto> Pipelines { get; set; } = new List<PipelineDto>();
        public List<TriggerDto> Triggers { get; set; } = new List<TriggerDto>();
        public DataFactoryConfigDto? DataFactoryConfig { get; set; }
    }
}
