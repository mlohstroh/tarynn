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


        public static TodoTask BuildTask(JsonData data)
        {
            TodoTask t = null;
            switch (data["Type"].ToString())
            {
                case "one_time":
                    t = new OneTimeTodoTask();
                    ((OneTimeTodoTask)t).Deadline = DateTime.Parse(data["Deadline"].ToString());
                    break;
            }
            if(data["Description"] != null)
                t.Description = data["Description"].ToString();
            t.Type = data["Type"].ToString();
            if (data["Size"] != null)
                t.Size = int.Parse(data["Size"].ToString());
            if (data["Title"] != null)
                t.Title = data["Title"].ToString();

            return t;
        }
    }
}
