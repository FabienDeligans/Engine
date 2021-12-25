using System.Collections.Generic;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace EngineTests.Base.Model
{
    public class EntityContainManyEntity : Entity
    {
        public string? Data { get; set; }

        [BsonIgnore]
        public List<EntityContainForeignKey>? CollectionOfEntityContainForeignKeys { get; set; }
    }
}
