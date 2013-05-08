using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Exceptions
{
    public class TException : Exception
    {
        public TException(string message)
            : base(message)
        {

        }
    }
}
