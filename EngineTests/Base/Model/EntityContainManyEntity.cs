using System.Collections.Generic;
using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace EngineTests.Base.Model
{
    public class EntityContainManyEntity : Entity
    {
        [BsonId(IdGenerator = typeof(IdGenerator<EntityContainManyEntity>))]
        public override string Id { get; set; }
        public string? Data { get; set; }

        [BsonIgnore]
        public List<EntityContainForeignKey>? CollectionOfEntityContainForeignKeys { get; set; }
    }
}
