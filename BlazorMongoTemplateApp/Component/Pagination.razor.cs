using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component
{
    public partial class Pagination<T>
    {
        [CascadingParameter]
        public Table<T> ParentTable { get; set; }

        private int PageSize { get; set; } = 10;
        public int CountData { get; set; }
        private List<int> Pages { get; set; }
        private int Index { get; set; }
        private int NbPage { get; set; }

        protected override void OnInitialized()
        {
            CountData = ParentTable.PaginationItems.Count();
            Index = 0;

            GetDisplayItems(Index);
        }

        private void GetDisplayItems(int index)
        {
            NbPage = (int)Math.Ceiling(CountData / (double)PageSize);
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
                    Pages = Pages.GetRange(Index - 4, nbPageDisplay > max ? maxDisplay : 11).ToList();
                    break;
                }
            }

            Index = index - 1;
            if (Index < 0) Index = 0;
            if (Index > NbPage) Index = NbPage - 1;

            ParentTable.DisplayItems = ParentTable.PaginationItems.Skip(Index * PageSize).Take(PageSize);
            ParentTable.RefreshMe();
        }

        private void ChangePageSize()
        {
            if (PageSize < 5)
            {
                PageSize = 5;
            }
            Index = 1;
            GetDisplayItems(Index);
        }

        public void RefreshMe()
        {
            OnInitialized();
            StateHasChanged();
        }
    }
}
