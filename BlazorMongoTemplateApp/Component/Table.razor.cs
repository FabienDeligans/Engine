using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component
{
    public partial class Table<T>
    {
        [Parameter]
        public IEnumerable<T> ItemsParameter { get; set; }
        public IEnumerable<T> PaginationItems { get; set; }
        public IEnumerable<T> SearchItems { get; set; }
        public IEnumerable<T> DisplayItems { get; set; }

        [Parameter]
        public RenderFragment THead { get; set; }

        [Parameter]
        public RenderFragment<T> TBody { get; set; }

        [Parameter]
        public bool WithPagination { get; set; }
        
        [Parameter]
        public Func<T, string> SearchOn { get; set; }

        public Pagination<T> PaginationComponent { get; set; }

        private bool SortByAscending { get; set; }

        protected override void OnInitialized()
        {
            DisplayItems = ItemsParameter;
            PaginationItems = DisplayItems; 
        }

        public void RefreshMe()
        {
            StateHasChanged();
        }
        
        public void OrderBy(string orderBy)
        {
            ItemsParameter = SortByAscending ?
                ItemsParameter.OrderBy(v => v.GetType().GetProperty(orderBy)?.GetValue(v)) :
                ItemsParameter.OrderByDescending(v => v.GetType().GetProperty(orderBy)?.GetValue(v));
            SortByAscending = !SortByAscending;

            DisplayItems = ItemsParameter;
            PaginationItems = DisplayItems;

            PaginationComponent.RefreshMe();  

            StateHasChanged();
        }
    }
}
