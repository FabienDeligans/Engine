using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models
{
    public class Exemplaire : Entity
    {
        public string Libelle { get; set; }

        [ForeignKey(typeof(Outillage))]
        public string OutillageId { get; set; }

        [BsonIgnore]
        public Outillage Outillage { get; set; }
    }
}
