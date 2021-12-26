using Engine.Model;
using System;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models
{
    /// <summary>
    /// Entity class Test
    /// </summary>
    public class MyEntity : Entity
    {
        [BsonElement("numeric")]
        public int Numeric { get; set; }
        
        [BsonElement("data")]
        public string Data { get; set; }

        [BsonElement("now")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Now { get; set; }
    }
}
