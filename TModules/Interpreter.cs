using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using TModules.DefaultModules;
using System.Reflection;
using LitJson;
using Analytics;
using WitAI;

namespace TModules
{
    class Interpreter
    {
        private ModuleManager _host;

        public static void Main(string[] args)
        {
            Console.WriteLine("TModule CLI");

            Interpreter inter = new Interpreter();

            inter.Start ();
            Console.Read();
        }

        public Interpreter()
        {
            _host = new ModuleManager();
        }

        public void Start()
        {
            string line = Console.ReadLine();


            JsonData data = null;
            //I should probably move this out of the constructor... whatever for now
            while (line != "q")
            {
                _host.RespondTo(line);

                line = Console.ReadLine();
            }
        }
    }
}