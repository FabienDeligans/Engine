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
        public bool PerPageParameter { get; set; }
        public bool SearchParameter { get; set; }
        public bool PaginationParameter { get; set; }
        public bool AccordionParameter { get; set; }
        public bool WithTabParameter { get; set; }

        protected override void OnInitialized()
        {
            PerPageParameter = true;
            SearchParameter = true;
            PaginationParameter = true;
            AccordionParameter = true;
            WithTabParameter = true; 

            Drop();
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
                ReturnedObject = result.Data;
            }

            await InvokeAsync(StateHasChanged);
        }

        private void Generate()
        {
            using var context = ContextFactory.MakeContext(); 
            
            var listOutillage = new List<Outillage>();
            for (var i = 0; i < 200; i++)
            {
                listOutillage.Add(new Outillage
                {
                    Libelle = RandomString(10),
                    Nb = new Random().Next(0, 11)
                });
            }

            context.InsertAll(listOutillage);
            Outillages = context.QueryCollection<Outillage>().ToList();

            var listExemplaire = new List<Exemplaire>(); 
            foreach (var outillage in Outillages)
            {
                for (var i = 0; i < 3; i++)
                {
                    listExemplaire.Add(new Exemplaire
                    {
                        Libelle = RandomString(5), 
                        Nb = new Random().Next(0,11), 
                        OutillageId = outillage.Id
                    });
                }
            }
            context.InsertAll(listExemplaire);
            Exemplaires = context.QueryCollection<Exemplaire>().ToList(); 

            InvokeAsync(StateHasChanged);
        }

        private void Drop()
        {
            using var context = ContextFactory.MakeContext(); 
            context.DropCollection<Outillage>();
            context.DropCollection<Exemplaire>();

            InvokeAsync(StateHasChanged);
        }
    }
}
