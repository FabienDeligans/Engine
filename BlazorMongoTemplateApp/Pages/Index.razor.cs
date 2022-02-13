using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using BlazorMongoTemplateApp.Component;
using BlazorMongoTemplateApp.Component.Modal.Called;
using BlazorMongoTemplateApp.Component.Modal.Caller;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index : ModalComponentCaller
    {
        public List<Outillage> Outillages { get; set; } = new List<Outillage>();
        public List<Exemplaire> Exemplaires { get; set; } = new List<Exemplaire>();
        public TableComponent<Outillage> ChildComponentOutillage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(Init); 
        }

        private void Init()
        {
            using var context = ContextFactory.MakeContext();
            context.DropCollection<Outillage>();
            context.DropCollection<Exemplaire>();

            var outillages = new List<Outillage>(); 
            var exemplaires = new List<Exemplaire>();

            for (int i = 0; i < 100_000; i++)
            {
                var outillage = new Outillage
                {
                    Libelle = RandomString(5),
                    Nb = new Random().Next(0, 11),
                }; 
                outillages.Add(outillage);
            }
            context.InsertAll(outillages);
            Outillages = context.QueryCollection<Outillage>().ToList();

            foreach (var outillage in Outillages)
            {
                for (int i = 0; i < 3; i++)
                {
                    var exemplaire = new Exemplaire
                    {
                        OutillageId = outillage.Id, 
                        Libelle = RandomString(10),
                        Nb = new Random().Next(0, 11),
                    };
                    exemplaires.Add(exemplaire);
                }
            }
            context.InsertAll(exemplaires);
            Exemplaires = context.QueryCollection<Exemplaire>().ToList();
        }

        private static readonly Random Random = new();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public override async Task ShowModal<T>(string dataId, string title)
        {
            var parameters = new ModalParameters();
            if (dataId != null)
            {
                parameters.Add("DataId", dataId);
            }

            var modal = Modal.Show<T>(title, parameters);
            var result = await modal.Result;

            if (result.Cancelled)
            {
                Modal.Show<OperationCancelled>();
            }
            else
            {
                DataReturned = result.Data;
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
