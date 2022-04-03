using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models
{
    public class Exemplaire : Entity
    {
        [BsonId(IdGenerator = typeof(IdGenerator<Exemplaire>))]
        public override string Id { get; set; }
        public string Libelle { get; set; }
        public int Nb { get; set; }

        [ForeignKey(typeof(Outillage))]
        public string OutillageId { get; set; }

        [BsonIgnore]
        public Outillage Outillage { get; set; }

    }
}
