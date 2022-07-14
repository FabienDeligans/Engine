using System;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component
{
    public partial class Search<T>
    {
        public string Filter { get; set; }

        [CascadingParameter]
        public Table<T> ParentTable { get; set; }

        private int CountSearch { get; set; }

        protected override void OnInitialized()
        {
            CountSearch = ParentTable.ItemsParameter.Count();
        }

        private void SearchDatas()
        {

            InvokeAsync(() =>
            {
                var filterFunc = ParentTable.SearchOn;

                if (!string.IsNullOrEmpty(Filter))
                {
                    ParentTable.SearchItems = ParentTable.ItemsParameter
                        .AsEnumerable()
                        .Where(x => (filterFunc(x) ?? "")
                            .Contains(Filter, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    ParentTable.SearchItems = ParentTable.ItemsParameter.AsEnumerable();
                }

                ParentTable.PaginationItems = ParentTable.SearchItems;
                CountSearch = ParentTable.SearchItems.Count();
                ParentTable.PaginationComponent.RefreshMe();
                ParentTable.RefreshMe();
            });

        }

        private void Clear()
        {
            Filter = "";
            CountSearch = ParentTable.ItemsParameter.Count();
            OnInitialized();
            ParentTable.PaginationComponent.RefreshMe();
            ParentTable.RefreshMe();
        }


    }
}
