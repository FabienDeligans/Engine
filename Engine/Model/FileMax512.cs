using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Engine.Model
{
    public class FileMax512 : Entity
    {
        public string Name { get; set; }
        public string Type{ get; set; }
        public long Size { get; set; }

        [Required]
        public string Data { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationDate { get; set; }
    }
}
