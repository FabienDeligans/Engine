using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Component;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index
    {
        public List<Outillage> Outillages { get; set; } = new List<Outillage>();
        public List<Exemplaire> Exemplaires { get; set; } = new List<Exemplaire>();
        public TableComponent<Outillage> ChildComponentOutillage { get; set; }

        protected override void OnInitialized()
        {
            using var context = ContextFactory.MakeContext(); 

            for (int i = 0; i < 50; i++)
            {
                var outillage = new Outillage
                {
                    Libelle = RandomString(10)
                }; 
                Outillages.Add(outillage);
                context.Insert(outillage);

                for (int j = 0; j < 3; j++)
                {
                    var exemplaire = new Exemplaire
                    {
                        OutillageId = outillage.Id,
                        Libelle = RandomString(5)
                    };
                    Exemplaires.Add(exemplaire);
                    context.Insert(exemplaire);
                }
            }
        }

        private static readonly Random Random = new();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private List<Exemplaire> GetExemplaire(string contextId)
        {
            throw new NotImplementedException();
        }

        public bool Open;


        private void Display(string contextId)
        {
            Open = !Open;
            id = contextId; 
        }

        public string id; 
    }
}
