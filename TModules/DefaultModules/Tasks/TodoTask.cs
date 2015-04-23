using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace TModules.DefaultModules.Tasks
{
    public class TodoTask
    {
        public string Title { get; protected set; }
        public DateTime Due { get; protected set; }

        public TodoTask(string name, DateTime due)
        {
            Title = name;
            Due = due;
        }
    }
}
