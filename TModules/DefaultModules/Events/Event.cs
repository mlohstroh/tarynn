using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules.DefaultModules.Events
{
    public class Event
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<EventInstance> Instances { get; set; }

        public Event(string name, int count)
        {
            this.Name = name;
            this.Count = count;
            Instances = new List<EventInstance>();
        }
    }
}
