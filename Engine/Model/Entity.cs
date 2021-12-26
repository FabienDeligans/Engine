using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Engine.Model
{
    public class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("isDisabled")]
        public bool IsDisabled { get; set; }
    }
}
