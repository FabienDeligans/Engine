﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorMongoTemplateApp.Component
{
    public partial class TableComponent<T>
    {
        [Parameter, EditorRequired]
        public RenderFragment TableHeader { get; set; }

        [Parameter, EditorRequired]
        public RenderFragment<T> RowTemplate { get; set; }

        [Parameter]
        public RenderFragment<T> RenderFragmentCollapse { get; set; }

        [Parameter, EditorRequired]
        public IEnumerable<T> CustomItems { get; set; }

        [Parameter]
        public bool Search { get; set; }

        [Parameter]
        public Func<T, string> GetFilterableText { get; set; }

        [Parameter]
        public bool Pagination { get; set; }

        [Parameter]
        public bool PerPage { get; set; }

        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter]
        public bool Accordion { get; set; }

        [Parameter]
        public int NbCol { get; set; }

        [Parameter]
        public bool WithTab { get; set; }

        private IEnumerable<T> Items { get; set; }
        public int Quantity { get; set; }
        private bool SortByAscending { get; set; }
        private int NbPage { get; set; }
        private int Index { get; set; } = 0;
        private int Count { get; set; }
        private List<int> Pages { get; set; }

        private string _filter;
        private T _item;
        public bool Collapse;
        private System.Timers.Timer timerObj;

        protected override void OnInitialized()
        {
            NbCol = typeof(T).GetProperties().Length;
            if (WithTab)
            {
                NbCol --; 
            }

            Items = CustomItems;
            Quantity = Items.Count();

            timerObj = new System.Timers.Timer(500);
            timerObj.Elapsed += OnUserFinish;
            timerObj.AutoReset = false;
        }

        public void RefreshMe()
        {
            OnInitialized();
            InvokeAsync(StateHasChanged);
        }

        private void OnValueChange(KeyboardEventArgs e)
        {
            timerObj.Stop();
            timerObj.Start();
        }

        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                var filterFunc = GetFilterableText;

                if (!string.IsNullOrEmpty(_filter))
                {
                    Items = Items
                        .AsEnumerable()
                        .Where(x => (filterFunc(x) ?? "")
                            .Contains(_filter, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    Items = Items;
                }

                Quantity = Items.Count();

                Index = 0;
                StateHasChanged();
            });
        }

        public void Display(T item)
        {
            Collapse = !Collapse;
            _item = item;
            StateHasChanged();
        }
        
        private void Clear()
        {
            Quantity = CustomItems.Count();
            _filter = "";
            StateHasChanged();
        }

        public void SortBy(Expression<Func<T, object>> predicate)
        {
            Items = CustomItems;

            Items = SortByAscending ? 
                Items.AsQueryable().OrderBy(predicate) : 
                Items.AsQueryable().OrderByDescending(predicate); 

            SortByAscending = !SortByAscending;
            StateHasChanged();
        }

        private void InitDataPagination()
        {
            Count = Items.Count();
            NbPage = (int)Math.Ceiling(Count / (double)PageSize);
        }

        private void GenerateButton()
        {
            InitDataPagination();

            Pages = new List<int>();
            var min = Index - 4 <= 1 ? 1 : Index - 4;
            var max = Index + 5 >= NbPage ? NbPage : Index + 6 <= 10 ? 10 : Index + 6;
            for (var i = min; i <= max; i++)
            {
                Pages.Add(i);
            }
        }

        private void ChangePage(int numPage)
        {
            Index = numPage - 1;
            if (Index < 0) Index = 0;
            if (Index > NbPage) Index = NbPage - 1;

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
