using System.Linq;
using BlazorMongoTemplateApp.Database;
using Engine.Model;
using MongoDB.Bson.Serialization;

namespace BlazorMongoTemplateApp.Models.TestModelCommande
{
    public class GeneratorId<T> : IIdGenerator where T : Entity
    {
        public object GenerateId(object container, object document)
        {
            using var context = ContextFactory.MakeContext();
            var count = context.QueryCollection<T>().Count();

            return $@"{typeof(T).Name}-{count + 1}"; 

        }

        public bool IsEmpty(object id)
        {
            return id == null || string.IsNullOrEmpty(id.ToString());
        }
    }
}
