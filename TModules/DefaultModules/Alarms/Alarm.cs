using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TModules
{
    [BsonIgnoreExtraElements]
    public class Alarm
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }

        public Alarm(DateTime time)
        {
            Time = time;
        }
    }
}
