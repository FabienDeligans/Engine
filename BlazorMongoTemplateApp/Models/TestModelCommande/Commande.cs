using System;
using System.ComponentModel.DataAnnotations;
using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models.TestModelCommande
{
    public class Commande : Entity
    {
        [BsonId(IdGenerator = typeof(GeneratorId<Commande>))]
        public override string Id { get; set; }

        [Required]
        [ForeignKey(typeof(Client))]
        public string ClientId { get; set; }

        [BsonIgnore]
        public Client Client { get; set; }

        [Required]
        [ForeignKey(typeof(ArticleCommande))]
        public string ArticleId { get; set; }

        [BsonIgnore]
        public ArticleCommande ArticleCommande { get; set; }

        [Required]
        [ForeignKey(typeof(Depot))]
        public string DepotId { get; set; }

        [BsonIgnore]
        public Depot Depot{ get; set; }

        [Required]
        public int QuantiteFacture { get; set; }

        [Required]
        public double PrixUnitaire { get; set; }

        public int QuantiteOfferte { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime DateDeLivraisonPrevue { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime DateDeLivraisonEffectuee { get; set; }

        public bool Valide { get; set; }

        public string Memo { get; set; }

        public EtatFacture EtatFacture { get; set; }
    }

    public enum EtatFacture
    {
        _,
        Payé, 
        PasRéglé, 
        EnCours
    }
}
