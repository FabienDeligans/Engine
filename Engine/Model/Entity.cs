using System;
using System.Runtime.CompilerServices;
using MongoDB.Bson.Serialization.Attributes;

namespace Engine.Model
{
    public abstract class Entity : IEntity
    {
        [BsonIgnore]
        public abstract string Id { get; set; }
        public bool IsDisabled { get; set; }
        
    }
}
