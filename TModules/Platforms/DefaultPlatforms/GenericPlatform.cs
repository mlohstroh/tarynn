using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules
{
    public class GenericPlatform : Platform
    {
        public GenericPlatform(PlatformManager m)
            : base("generic", m)
        {
            
        }
    }
}
