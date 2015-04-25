using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules
{
    public class DuplicatePlatformException : Exception
    {
        public DuplicatePlatformException() : base("A duplicate platform is trying to be added")
        {
        }
    }

    public class DuplicateResourceResponderException : Exception
    {
        public DuplicateResourceResponderException()
            : base("A duplicate resource respondcer is trying to be added")
        {
        }
    }
}
