using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace BlazorMongoTemplateApp.Component
{
    public partial class Table<T>
    {
        [Parameter]
        [Required]
        public RenderFragment Header { get; set; }

        [Parameter]
        [Required]
        public RenderFragment<T> Row { get; set; }

        public Pagination<T> PaginationComponent { get; set; }

        [Parameter]
        public IEnumerable<T> CollectionT { get; set; }
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<T> ItemsPagination { get; set; }
        public int PageSize { get; set; } = 10;

        protected override void OnInitialized()
        {
            Items = new List<T>();
            ItemsPagination = new List<T>();

            Items = CollectionT;
            ItemsPagination = Items;
        }


        // sort
        private bool SortByAscending { get; set; }
        public void Sort(string sortBy)
        {
            Items = SortByAscending
                ? CollectionT.OrderBy(v => v.GetType().GetProperty(sortBy)?.GetValue(v))
                : CollectionT.OrderByDescending(v => v.GetType().GetProperty(sortBy)?.GetValue(v));
            SortByAscending = !SortByAscending;

            ItemsPagination = Items;
            virtualizeComponent.RefreshDataAsync();
            PaginationComponent.RefreshMe();
            StateHasChanged();
        }

        //// filter
        //public Func<T, string> GetFilterableText { get; set; }

        //private string Filter;
        //private void Search()
        //{
        //    var properties = typeof(T).GetProperties();
        //    var filteredCollectionT =
        //    (from item
        //            in CollectionT
        //     from property
        //         in properties
        //     where property.GetValue(item) != null
        //     where property.GetValue(item)!.ToString()!.ToUpper().Contains(Filter.ToUpper())
        //     select item);

        //    Items = filteredCollectionT;
        //    ItemsPagination = Items;

        //    PaginationComponent.RefreshMe();

        //    virtualizeComponent.RefreshDataAsync();
        //    InvokeAsync(StateHasChanged);
        //}

        //private void Clear()
        //{
        //    Filter = "";
        //    RefreshParent();
        //    OnInitialized();
        //    StateHasChanged();

        //}

        private Virtualize<T> virtualizeComponent;

        private async ValueTask<ItemsProviderResult<T>> LoadItems(ItemsProviderRequest request)
        {
            var i = Items.Count(); 
            if (Items.Count() > PageSize)
            {
                i = PageSize; 
            }
            return new ItemsProviderResult<T>(Items, i);
        }

        public void RefreshParent()
        {
            virtualizeComponent.RefreshDataAsync();
            InvokeAsync(StateHasChanged);
        }
    }
}
