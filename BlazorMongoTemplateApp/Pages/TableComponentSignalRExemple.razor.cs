using BlazorMongoTemplateApp.Component;
using BlazorMongoTemplateApp.Database;
using Engine.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Models;

namespace BlazorMongoTemplateApp.Pages
{
    // TODO inherit from SignalRDataList<T> in .razor file
    // When Initialize override "Context" and 
    // run await InitSignalR()
    public partial class TableComponentSignalRExemple
    {
        // TODO not forget this to ref
        private TableComponent<MyEntity> ChildComponent { get; set; }
        private int Number { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Context = ContextFactory.MakeContext();
            await InitSignalR();
        }

        private async Task Generate()
        {
            using var context = ContextFactory.MakeContext();

            var list = new List<MyEntity>();
            for (var i = 0; i < Number; i++)
            {
                list.Add(new MyEntity
                {
                    Data = RandomString(10),
                    Now = DateTime.Now,
                    Numeric = new Random().Next(0, 11)
                });
            }
            context.InsertAll(list);
            await InvokeAsync(StateHasChanged);
            await OnInitializedAsync();
        }

        private static readonly Random Random = new();

        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private async Task Drop()
        {
            using var context = ContextFactory.MakeContext();
            context.DropCollection<MyEntity>();

            await RefreshSignalR(null, Crud.Read);
        }

        private async Task Add()
        {
            var entity = new MyEntity
            {
                Numeric = new Random().Next(0, 11),
                Data = RandomString(10),
                Now = DateTime.Now
            };
            using var context = ContextFactory.MakeContext();
            context.Insert(entity);

            await RefreshSignalR(entity, Crud.Create);
        }

        private async Task Update(MyEntity entity)
        {
            using var context = ContextFactory.MakeContext();
            context.UpdateEntity(entity);

            await RefreshSignalR(entity, Crud.Update);
        }

        private async Task Delete(MyEntity entity)
        {
            using var context = ContextFactory.MakeContext();
            context.RemoveOne<MyEntity>(v => v.Id == entity.Id);

            await RefreshSignalR(entity, Crud.Delete);
        }

    }
}
