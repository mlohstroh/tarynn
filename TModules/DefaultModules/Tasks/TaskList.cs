using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TModules
{
    [BsonIgnoreExtraElements]
    public class TaskList
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string ListTitle { get; set; }

        [BsonElement]
        public List<string> Items { get; set; }

        public TaskList (string title, List<string> items)
        {
            ListTitle = title;
            Items = items;
        }

        public TaskList() { }
    }
}

