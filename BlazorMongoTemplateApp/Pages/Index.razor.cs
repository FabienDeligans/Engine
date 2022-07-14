using System;
using System.Threading.Tasks;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index
    {
        private string Date { get; set; }
        private string Time { get; set; }
        protected override void OnInitialized()
        {
            var timer = new System.Threading.Timer((_) =>
            {
                InvokeAsync(async () =>
                {
                    Date = DateTime.Now.ToLongDateString();
                    Time = DateTime.Now.ToLongTimeString(); 
                    
                    await InvokeAsync(StateHasChanged);
                });
            }, null, 0, 500);
        }
    }
}
