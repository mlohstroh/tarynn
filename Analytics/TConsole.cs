using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analytics
{
    public class TConsole
    {
        private string _name;

        public TConsole(Type t)
        {
            _name = t.Name;
        }

        public TConsole(string loggerName)
        {
            _name = loggerName;
        }

        public void Info(string info)
        {
            Console.WriteLine (Message (info), "INFO");
        }

        public void InfoFormat(string info, params object[] args)
        {
            Console.WriteLine (Message (string.Format(info, args)), "INFO");
        }

        public void Error(string err)
        {
            Console.WriteLine (Message (err), "ERROR");
        }

        public void ErrorFormat(string info, params object[] args)
        {
            Console.WriteLine (Message (string.Format(info, args)), "ERROR");
        }

        public void Debug(string debug)
        {
            Console.WriteLine (Message (debug), "DEBUG");
        }

        public void DebugFormat(string info, params object[] args)
        {
            Console.WriteLine (Message (string.Format(info, args)), "DEBUG");
        }

        private string PrettyDate()
        {
            return DateTime.Now.ToString ("u");
        }

        private string Message(string message, string level = "DEBUG")
        {
            return string.Format ("{0} {1} [{2}] {3}", PrettyDate(), _name, level, message);
        }
    }
}
