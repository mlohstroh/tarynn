using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    public abstract class MethodPackage
    {
        private List<string> methodNames = new List<string>();

        public MethodPackage(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
