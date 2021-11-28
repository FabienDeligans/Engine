using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace EngineTests.Base.Model
{
    public class EntityContainForeignKey : Entity
    {
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
