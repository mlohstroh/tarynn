using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TModules.DefaultModules.Tasks
{
    [BsonIgnoreExtraElements]
    public class TodoTask
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string Title { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Due { get; set; }

        public TodoTask(string name, DateTime due)
        {
            Title = name;
            Due = due;
        }
    }
}
