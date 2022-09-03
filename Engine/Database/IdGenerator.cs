using System;
using MongoDB.Bson.Serialization;

namespace Engine.Database
{
    public abstract class IdGenerator<T> : IIdGenerator
    {
        public virtual object GenerateId(object container, object document)
        {
            var date = DateTime.Now;
            return $@"{typeof(T).Name}-{date.ToLocalTime()}-{date.Ticks}";
        }
        
        public bool IsEmpty(object id)
        {
            return id == null || String.IsNullOrEmpty(id.ToString());
        }

    }
}
