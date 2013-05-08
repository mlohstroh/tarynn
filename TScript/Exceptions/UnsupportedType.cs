using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Exceptions
{
    public class UnsupportedType : TException
    {
        public UnsupportedType()
            : base("Unsupported Type")
        {
        }
    }
}
