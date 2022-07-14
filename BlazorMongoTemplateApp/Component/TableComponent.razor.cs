using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    Items = Items.AsEnumerable();
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

        public void Sort(string property)
        {
            Items = CustomItems;
            Items = SortByAscending ?
                Items.OrderBy(v => v.GetType().GetProperty(property)?.GetValue(v)) :
                Items.OrderByDescending(v => v.GetType().GetProperty(property)?.GetValue(v));
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
            var min = Index - 5 <= 1 ? 1 : Index - 5;
            var max = Index + 5 >= NbPage ? NbPage : Index + 5 <= 10 ? 10 : Index + 5;
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
