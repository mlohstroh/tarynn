using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TScript.Methods
{
    public class FileMethods : MethodPackage
    {
        public FileMethods()
            : base("File Methods")
        {
            this.methodNames.Add("read_csv");
            this.methodNames.Add("write_csv");
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            return TObject.Empty();
        }

        public override TObjectChange GetResultForMethod(string method, object[] args)
        {
            TObjectChange change = new TObjectChange();
            switch (method)
            {
                case "read_csv":
                    HandleReadCsv(args);
                    break;
                case "write_csv":
                    HandleWriteCsv(args);
                    break;
            }
            return change;
        }

        private TObjectChange HandleReadCsv(object[] args)
        {
            string csvBlob = File.ReadAllText(Host.GetObjectValue(args[0]).ToString());
            return Host.MakeChange((TObject)args[1], csvBlob);
        }

        private TObjectChange HandleWriteCsv(object[] args)
        {
            string csvBlob = Host.GetObjectValue(args[1]).ToString();

            File.WriteAllText(Host.GetObjectValue(args[0]).ToString(), csvBlob);

            return TObjectChange.Empty();
        }
    }
}
