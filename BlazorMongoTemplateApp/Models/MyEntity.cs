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
        public int Numeric { get; set; }
        public string Data { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Now { get; set; }
    }
}
