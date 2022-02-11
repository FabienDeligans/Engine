using BlazorMongoTemplateApp.Database;
using Engine.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Database;

namespace BlazorMongoTemplateApp.Component
{
    public partial class TableComponent<T>
    {
        [Parameter]
        public RenderFragment TableHeader { get; set; }

        [Parameter]
        public RenderFragment<T> RowTemplate { get; set; }

        private IEnumerable<T> Items { get; set; }

        [Parameter]
        public IEnumerable<T> CustomItems { get; set; }

        [Parameter]
        public Func<T, string> GetFilterableText { get; set; }

        [Parameter]
        public bool Pagination { get; set; }

        [Parameter]
        public bool Search { get; set; }

        public int Quantity { get; set; }

        private string _filter;
        private bool SortByAscending { get; set; }

        protected override void OnInitialized()
        {
            Items = CustomItems;
            Quantity = Items.Count();
        }

        private static readonly Func<T, string> DefaultGetFilterableText = item => item?.ToString() ?? "";

        private void FilterItems(ChangeEventArgs obj)
        {
            _filter = obj.Value?.ToString();

            var filterFunc = GetFilterableText ?? DefaultGetFilterableText;

            if (!string.IsNullOrEmpty(_filter))
            {
                Items = Items
                     .AsEnumerable()
                     .Where(x => (filterFunc(x) ?? "")
                         .Contains(_filter, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                Items = Items.AsEnumerable();
            }

            Quantity = Items.Count();

            Index = 0;
            StateHasChanged();
        }

        private void Clear()
        {
            Quantity = CustomItems.Count();

            _filter = "";
            StateHasChanged();
        }

        public void Sort(string property)
        {
            Items = CustomItems;

            Items = SortByAscending ?
                Items.OrderBy(v => v.GetType().GetProperty(property)?.GetValue(v)) :
                Items.OrderByDescending(v => v.GetType().GetProperty(property)?.GetValue(v));

            SortByAscending = !SortByAscending;

            StateHasChanged();
        }

        [Parameter]
        public bool PerPage { get; set; }

        [Parameter]
        public int PageSize { get; set; } = 10;
        private int NbPage { get; set; }
        private int Index { get; set; } = 0;
        private int Count { get; set; }
        private List<int> Pages { get; set; }
        


        private void InitDataPagination()
        {
            Count = Items.Count();
            NbPage = (int)Math.Ceiling(Count / (double)PageSize);
        }

        private void GenerateButton()
        {
            InitDataPagination();

            Pages = new List<int>();
            for (var i = 0; i < NbPage; i++)
            {
                Pages.Add(i + 1);
            }

            var min = 0;
            var max = NbPage + 2;

            if (NbPage <= 10) return;
            switch (Index)
            {
                case < 5:
                    Pages = Pages.GetRange(min, 10).ToList();
                    break;
                case >= 5:
                    {
                        var nbPageDisplay = Index + 9;
                        var maxDisplay = max - Index + 3;
                        Pages = Pages.GetRange(Index - 5, nbPageDisplay > max ? maxDisplay : 11).ToList();
                        break;
                    }
            }
        }

        private void ChangePage(int numPage)
        {
            Index = numPage - 1;
            if (Index < 0) Index = 0;
            if (Index >= NbPage) Index = NbPage - 1;

            GetPage();
            StateHasChanged();
        }

        private IEnumerable<T> GetPage()
        {
            if (!Pagination) return Items;
            GenerateButton();
            return Items.Skip(Index * PageSize).Take(PageSize);
        }

        private void ChangePageSize(int pageSize)
        {
            PageSize = pageSize;
            Index = 0;
            Clear();
            OnInitialized();
        }


    }
}
