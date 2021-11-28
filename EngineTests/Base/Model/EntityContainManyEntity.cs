using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
