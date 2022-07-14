#nullable enable
using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;
using MudBlazor;

namespace BlazorMongoTemplateApp.Models
{
    public class Outillage : Entity
    {
        [BsonId(IdGenerator = typeof(IdGenerator<Outillage>))]
        public override string Id { get; set; }
        public string Libelle { get; set; }
        public int Nb { get; set; }

    }
}
