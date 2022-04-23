using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace BlazorMongoTemplateApp.Component
{
    public partial class Table<T>
    {
        [Parameter]
        public RenderFragment Header { get; set; }

        [Parameter]
        public RenderFragment<T> Row { get; set; }

        [Parameter]
        public IEnumerable<T> CollectionT { get; set; }
        public IEnumerable<T> Items { get; set; }



        protected override void OnInitialized()
        {
            Items = CollectionT;
        }

        // sort
        private bool SortByAscending { get; set; }
        public void Sort(string sortBy)
        {
            Items = SortByAscending
                ? Items.OrderBy(v => v.GetType().GetProperty(sortBy)?.GetValue(v))
                : Items.OrderByDescending(v => v.GetType().GetProperty(sortBy)?.GetValue(v));
            SortByAscending = !SortByAscending;
            virtualizeComponent.RefreshDataAsync();

            StateHasChanged();
        }

        // filter
        public Func<T, string> GetFilterableText { get; set; }

        private string Filter;
        private void Search()
        {
            var properties = typeof(T).GetProperties();
            var filteredCollectionT =
            (from item
                    in CollectionT
             from property
                 in properties
             where property.GetValue(item) != null
             where property.GetValue(item)!.ToString()!.ToUpper().Contains(Filter.ToUpper())
             select item);

            Items = filteredCollectionT;
            virtualizeComponent.RefreshDataAsync(); 
            StateHasChanged();
        }

        private void Clear()
        {
            Filter = "";
            Items = CollectionT;
            StateHasChanged();
        }

        private Virtualize<T> virtualizeComponent;

        private async ValueTask<ItemsProviderResult<T>> LoadItems(ItemsProviderRequest request)
        {
            var items = Items;

            return new ItemsProviderResult<T>(items, items.Count());
        }
    }
}
