using DataFactoryViewer.Data;

namespace DataFactoryViewer.Utils
{
    public interface IJsonToBicepConverter
    {
        string Convert(string resourceName, DataFactoryObjectType ObjectType, string json);
    }
}