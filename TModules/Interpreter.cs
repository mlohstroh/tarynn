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

namespace TModules
{
    class Interpreter
    {
        private ModuleManager _host;

        public static void Main(string[] args)
        {
            Console.WriteLine("TModule CLI");

            Interpreter inter = new Interpreter();

            Console.Read();
        }

        public Interpreter()
        {
            _host = new ModuleManager();

            string line = Console.ReadLine();


            JsonData data;
            //I should probably move this out of the constructor... whatever for now
            while (line != "q")
            {
                string[] response = ParseLine(line);

                if (response.Length != 3)
                {
                    TConsole.Error("ERROR IN PARSING");
                    break;
                }
                Type module = _host.GetModuleByName(response[0]).GetType();

                MethodInfo info = module.GetMethod(response[1]);
                if (info == null)
                {
                    TConsole.Error(string.Format("No method {0} found on module {1}", response[1], response[0]));
                }
                else
                {
                    data = info.Invoke(this, new object[] { response[2] }) as JsonData;
                }

                line = Console.ReadLine();
            }
        }

        private string[] ParseLine(string line)
        {
            string[] paramsParsed = line.Split(new char[] { '.', '(', ')' });

            Array.Resize<string>(ref paramsParsed, paramsParsed.Length - 1);

            return paramsParsed;
        }
    }
}