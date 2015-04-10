using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAI
{
    public class WitException : Exception
    {
        public WitException(string message) : base(message) { }
    }
}
