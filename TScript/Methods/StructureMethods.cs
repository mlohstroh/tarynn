using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    class StructureMethods : MethodPackage
    {
        public StructureMethods()
            : base("File Methods")
        {
            this.supportedTypes.Add("dict");
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            TObject obj = new TObject();
            obj.Name = name;
            obj.InnerType = type;

            switch (type)
            {
                case "dict":
                    obj.Value = new Dictionary<string, object>();
                    break;
            }

            return obj;
        }

        public override TObjectChange GetResultForMethod(string method, object[] args)
        {
            TObjectChange change = new TObjectChange();
            switch (method)
            {

            }
            return change;
        }
    }
}
