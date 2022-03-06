using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
