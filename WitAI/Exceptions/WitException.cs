using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WitAI
{
    public class WitException : Exception
    {
        public WitException(string message, HttpStatusCode code) : base(string.Format("Status Code: {0} Message: {1}", code, message)) { }
    }
}
