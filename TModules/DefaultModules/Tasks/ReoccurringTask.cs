using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules.DefaultModules.Tasks
{
    class ReoccurringTask : TodoTask
    {
        public List<int> DaysToRepeat { get; set; }
    }
}
