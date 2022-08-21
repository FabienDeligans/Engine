using System.ComponentModel.DataAnnotations;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models.TestModelCommande
{
    public class ArticleCommande : Entity
    {
        [BsonId(IdGenerator = typeof(GeneratorId<ArticleCommande>))]
        public override string Id { get; set; }

        [Required] 
        public string Libelle { get; set; }

        [Required]
        public double PrixUnitaire { get; set; }

        public int QtMini { get; set; }
    }
}
