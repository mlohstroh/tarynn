using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analytics;

namespace TScript.Methods
{
    public abstract class MethodPackage
    {
        protected List<string> methodNames = new List<string>();
        protected List<string> supportedTypes = new List<string>();

        public MethodPackage(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public bool MethodExists(string method)
        {
            return methodNames.Contains(method);
        }

        /// <summary>
        /// Gets the result for the specified method and arguments
        /// </summary>
        /// <param name="method">The method name</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns>The variable name and the new value for it</returns>
        public abstract TObjectChange GetResultForMethod(string method, object[] args);

        /// <summary>
        /// Returns a new TObject with the initial values and names stored
        /// </summary>
        /// <param name="type">The type of object</param>
        /// <param name="name">The name of the object</param>
        /// <returns></returns>
        public abstract TObject GetNewObjectForType(string type, string name);

        public bool SupportsType(string type)
        {
            return supportedTypes.Contains(type);
        }

        public Interpreter Host { get; set; }
    }
}
