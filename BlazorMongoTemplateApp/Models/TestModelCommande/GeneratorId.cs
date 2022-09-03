using System.Linq;
using BlazorMongoTemplateApp.Database;
using Engine.CustomAttribute;
using Engine.Database;
using Engine.Model;

namespace BlazorMongoTemplateApp.Models.TestModelCommande
{
    public class GeneratorId<T> : IdGenerator<T> where T : Entity
    {
        public override object GenerateId(object container, object document)
        {
            using var context = ContextFactory.MakeContext();
            var count = context.QueryCollection<T>().Count();

            return $@"{typeof(T).Name}-{count + 1}";
        }
    }
}
