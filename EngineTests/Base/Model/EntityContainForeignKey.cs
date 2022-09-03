using Engine.CustomAttribute;
using Engine.Database;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace EngineTests.Base.Model
{
    public class EntityContainForeignKey : Entity
    {
        [BsonId(IdGenerator = typeof(IdGenerator<EntityContainForeignKey>))]
        public override string Id { get; set; }
        public string? Data { get; set; }

        [ForeignKey(typeof(MyEntity))]
        public string? MyEntityId { get; set; }

        [BsonIgnore]
        public MyEntity? MyEntity { get; set;}

        [ForeignKey(typeof(EntityContainManyEntity))]
        public string? EntityContainManyEntityId { get; set; }

        [BsonIgnore]
        public EntityContainManyEntity? EntityContainManyEntity { get; set; }

    }
}
