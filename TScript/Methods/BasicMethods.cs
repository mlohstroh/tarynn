using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    public class BasicMethods : MethodPackage
    {
        public BasicMethods()
            : base("Basic Methods")
        {
            this.methodNames.Add("rand");
            this.methodNames.Add("insert");
            this.methodNames.Add("mod");
            this.methodNames.Add("valueAt");
            this.supportedTypes.Add("integer");
            this.supportedTypes.Add("list");
        }

        public override string GetResultForMethod(string method, string[] args)
        {
            string newValue = "";

            switch (method)
            {
                case "rand":
                    break;
                case "insert":
                    break;
                case "mod":
                    break;
                case "valueAt":
                    break;
            }

            return newValue;
        }
    }
}
