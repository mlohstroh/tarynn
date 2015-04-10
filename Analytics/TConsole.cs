using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analytics
{
    public static class TConsole
    {
        public static void Info(string info)
        {
            Console.WriteLine("[INFO]: " + info);
        }

        public static void InfoFormat(string info, params object[] args)
        {
            Console.WriteLine("[INFO]: " + info, args);
        }

        public static void Error(string err)
        {
            Console.WriteLine("[ERROR]: " + err);
        }

        public static void ErrorFormat(string info, params object[] args)
        {
            Console.WriteLine("[ERROR]: " + info, args);
        }

        public static void Debug(string debug)
        {
            Console.WriteLine("[DEBUG]: " + debug);
        }

        public static void DebugFormat(string info, params object[] args)
        {
            Console.WriteLine("[DEBUG]: " + info, args);
        }
    }
}
