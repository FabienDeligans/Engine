using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Engine.Model
{
    public interface IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool IsDisabled { get; set; }
    }
}
