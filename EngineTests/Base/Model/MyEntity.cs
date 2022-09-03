using System;
using Engine.CustomAttribute;
using Engine.Database;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace EngineTests.Base.Model
{
    public class MyEntity : Entity
    {
        [BsonId(IdGenerator = typeof(IdGenerator<MyEntity>))]
        public override string Id { get; set; }
        public string? Name { get; set; }
        public int Numeric { get; set; }
        public DateTime DateTime { get; set; }
    }
}
