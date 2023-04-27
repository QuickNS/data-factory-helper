using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;

namespace DataFactoryViewer.Data
{
    public abstract class BaseDataFactoryObject
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _typeName;

        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        private string _json;

        public string Json
        {
            get { return _json; }
            set { _json = value; }
        }

        public BaseDataFactoryObject(SubResource resource, IAdfSerializer serializer)
        {
            Id = resource.Id;
            Name = resource.Name;
            Json = serializer.ToJson(resource);
        }
    }
}
