using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component
{
    public partial class Pagination<T>
    {
        [CascadingParameter]
        public Table<T>TableParent { get; set; }

        [Parameter]
        public IEnumerable<T> Items { get; set; }
        
        [Parameter]
        public int PageSize { get; set; } 

        private int NbPage { get; set; }
        private int Index { get; set; }

        private List<int>ListPages { get; set; }
        protected override void OnInitialized()
        {
            Index = 1; 
            NbPage = (int)Math.Ceiling(Items.Count() / (double)PageSize);
            GenerateButton(); 
            ChangePage(Index);
        }

        private void GenerateButton()
        {
            ListPages = new List<int>();
            for (int i = 1; i <= NbPage; i++)
            {
                ListPages.Add(i);
            }
        }

        private void ChangePage(int i)
        {
            Index = i - 1;
            var pageDisplay = Items.Skip(Index * PageSize).Take(PageSize);

            TableParent.Items = pageDisplay; 
            TableParent.RefreshParent();
        }
        
        public void RefreshMe()
        {
            OnInitialized();
            StateHasChanged();
        }
    }
}
