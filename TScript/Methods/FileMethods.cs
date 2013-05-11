using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    public class FileMethods : MethodPackage
    {
        public FileMethods()
            : base("File Methods")
        {
            this.methodNames.Add("open");
            this.methodNames.Add("close");
            this.methodNames.Add("read_csv");
            this.methodNames.Add("write_csv");

            this.supportedTypes.Add("file");
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            TObject obj = new TObject();
            obj.Name = name;
            obj.InnerType = type;

            switch (type)
            {
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
