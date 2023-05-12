namespace DataFactoryViewer.Utils
{
    public interface IAdfSerializer
    {
        string ToJson(object o);
        string ToAdfJson(object o);
    }
}