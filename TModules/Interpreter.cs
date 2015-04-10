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
            Wit wit = new Wit("2JXL3QOZV4SH2HOCB74FVPJUKMS3ETOO");
            wit.Query("check the weather for me");

            Interpreter inter = new Interpreter();

            Console.Read();
        }

        public Interpreter()
        {
            _host = new ModuleManager();

            string line = Console.ReadLine();


            JsonData data = null;
            //I should probably move this out of the constructor... whatever for now
            while (line != "q")
            {
                string[] response = ParseLine(line);

                if (response.Length < 2)
                {
                    TConsole.Error("ERROR IN PARSING");
                    break;
                }
                TModule module = _host.GetModuleByName(response[0]);
                Type moduleType = module.GetType();

                MethodInfo info = moduleType.GetMethod(response[1]);
                if (info == null)
                {
                    TConsole.Error(string.Format("No method {0} found on module {1}", response[1], response[0]));
                }
                else
                {
                    //rip out the parameters so we can pass along multiple params
                    object[] _params = new object[response.Length - 1];

                    for (int i = 2; i < response.Length; i++)
                    {
                        _params[i - 2] = response[i];
                    }
                    _params[_params.Length - 1] = data;
                    data = info.Invoke(module, _params) as JsonData;
                }

                line = Console.ReadLine();
            }
        }

        private string[] ParseLine(string line)
        {
            string[] dotParsed = line.Split(new char[] { '.', '(' });
            Array.Resize<string>(ref dotParsed, dotParsed.Length - 1);

            //, '(', ')', ',' 

            string[] parameters = line.Split(new char[] { '(', ')', ',' });

            for (int i = 0; i < parameters.Length - 1; i++)
            {
                parameters[i] = parameters[i + 1];
            }

            string[] mergedParameters = new string[parameters.Length - 2];

            for (int i = 0; i < mergedParameters.Length; i++)
            {
                mergedParameters[i] = parameters[i];
            }

            string[] fullParsed = new string[2 + mergedParameters.Length];

            fullParsed[0] = dotParsed[0];
            fullParsed[1] = dotParsed[1];

            for (int i = 2; i < fullParsed.Length; i++)
            {
                fullParsed[i] = mergedParameters[i - 2];
            }

            return fullParsed;
        }
    }
}