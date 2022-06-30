using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Engine.Model
{
    public class Fichier : Entity
    {
        public override string Id { get; set; }
        public string Name { get; set; }
        public byte[] DataBytes { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UploadDate { get; set; }
        public long Size { get; set; }

    }
}
