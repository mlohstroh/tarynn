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
        public int Size { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Done { get; set; }
        public DateTime TimeToNotify { get; set; } 

        public static TodoTask BuildTask(JsonData data)
        {
            TodoTask t = null;
            switch (data["Type"].ToString())
            {
                case "one_time":
                    t = JsonMapper.ToObject<OneTimeTodoTask>(data.ToJson());
                    break;
                case "reoccurring":
                    t = JsonMapper.ToObject<ReoccurringTask>(data.ToJson());
                    break;
            }

            return t;
        }
    }
}
