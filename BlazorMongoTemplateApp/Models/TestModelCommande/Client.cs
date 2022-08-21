using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models.TestModelCommande
{
    public class Client : Entity
    {
        [BsonId(IdGenerator = typeof(GeneratorId<Client>))]
        public override string Id { get; set; }

        [Required]
        public string Nom
        {
            get => nom.ToUpper();
            set => nom = value.ToUpper(); 
        }

        [BsonIgnore]
        private string nom = "";

        [Required]
        public string Prenom
        {
            get => new CultureInfo("fr-FR", false).TextInfo.ToTitleCase(prenom); 
            set => prenom = new CultureInfo("fr-FR", false).TextInfo.ToTitleCase(value);
        }

        [BsonIgnore]
        private string prenom = "";

        public string Ecurie
        {
            get => ecurie.ToUpper();
            set => ecurie = value.ToUpper();
        }

        [BsonIgnore]
        private string ecurie = "";

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

        [Required]
        [MaxLength(10)]
        public string Telephone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [ForeignKey(typeof(Depot))] 
        public string DepotId { get; set; }

        [BsonIgnore]
        public Depot Depot { get; set; }

        public JourDeTournee JourDeTournee { get; set; }

        public bool Abonnement { get; set; }

        public Commande CommanDeDeBase { get; set; } = new Commande(); 

        public string Memo { get; set; }
    }

    public enum JourDeTournee
    {
        Dimanche,
        Lundi,
        Mardi,
        Mercredi,
        Jeudi,
        Vendredi,
        Samedi,
    }
}
