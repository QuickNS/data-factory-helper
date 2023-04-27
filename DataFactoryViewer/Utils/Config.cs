namespace DataFactoryViewer.Utils
{
    public class Config
    {
        DataFactoryConfig _dataFactoryConfig;
        public Config(IConfiguration configuration)
        {
            _dataFactoryConfig = configuration.GetSection("DataFactory").Get<DataFactoryConfig>();
        }

        public DataFactoryConfig DataFactoryConfig { get { return _dataFactoryConfig; } }
    }
}
