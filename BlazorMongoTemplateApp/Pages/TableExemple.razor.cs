using System;
using System.Collections.Generic;
using System.Linq;
using BlazorMongoTemplateApp.Component;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TableExemple
    {
        public Table<Outillage>TableOutillage { get; set; }
        public Table<Exemplaire> TableExemplaire { get; set; }
        private IEnumerable<Outillage> Outillages { get; set; }
        private IEnumerable<Exemplaire> Exemplaires { get; set; }
        private int Number { get; set; }


        protected override void OnInitialized()
        {
            using var context = ContextFactory.MakeContext();
            Outillages = context.QueryCollection<Outillage>();
            Exemplaires = context.QueryCollection<Exemplaire>();
        }

        #region Génération

        private static readonly Random Random = new();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private void Generate()
        {
            using var context = ContextFactory.MakeContext();

            var listOutillage = new List<Outillage>();
            for (var i = 0; i < Number; i++)
            {
                listOutillage.Add(new Outillage
                {
                    Libelle = RandomString(10),
                    Nb = new Random().Next(0, 11),
                });
            }

            context.InsertAll(listOutillage);
            Outillages = context.QueryCollection<Outillage>();

            var listExemplaire = new List<Exemplaire>();
            foreach (var outillage in Outillages)
            {
                for (var i = 0; i < 3; i++)
                {
                    listExemplaire.Add(new Exemplaire
                    {
                        Libelle = RandomString(5),
                        Nb = new Random().Next(0, 11),
                        OutillageId = outillage.Id
                    });
                }
            }
            context.InsertAll(listExemplaire);
            Exemplaires = context.QueryCollection<Exemplaire>();

            TableOutillage?.RefreshMe();
            StateHasChanged();
        }

        private void Drop()
        {
            using var context = ContextFactory.MakeContext();
            context.DropDatabase();

            Outillages = null;
            Exemplaires = null;
            TableOutillage?.RefreshMe();
            StateHasChanged();
        }

        #endregion
    }
}
