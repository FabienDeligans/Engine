using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models.TestModelCommande
{
    public class Depot : Entity
    {
        [BsonId(IdGenerator = typeof(GeneratorId<Depot>))]
        public override string Id { get; set; }

        [Required]
        public string Libelle
        {
            get => new CultureInfo("fr-FR", false).TextInfo.ToTitleCase(libelle);
            set => libelle = new CultureInfo("fr-FR", false).TextInfo.ToTitleCase(value);
        }

        [BsonIgnore]
        private string libelle = "";

        [Required]
        public string Adresse { get; set; }

        [Required]
        public string CodePostal { get; set; }

        [Required]
        public string Ville
        {
            get => ville.ToUpper();
            set => ville = value.ToUpper();
        }

        [BsonIgnore]
        private string ville = "";

    }
}
