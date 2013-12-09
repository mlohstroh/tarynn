using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules.DefaultModules.Events
{
    public class EventInstance
    {
        public DateTime Occurred { get; set;}

        public EventInstance(DateTime t)
        {
            Occurred = t;
        }
    }
}
