using System;
using System.Threading.Tasks;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class AsyncTest
    {
        private DateTime Start { get; set; }
        private DateTime End { get; set; }
        private string Ac1 { get; set; }
        private string Ac2 { get; set; }

        private DateTime StartAsync { get; set; }
        private DateTime EndAsync { get; set; }
        private string Ac1Async { get; set; }
        private string Ac2Async { get; set; }


        private async void Do()
        {
            Start = DateTime.Now;
            
            Ac1 = await Action();
            Ac2 = await Action();

            End = DateTime.Now;
            
            StateHasChanged();
        }

        private async Task DoAsync()
        {
            StartAsync = DateTime.Now;

            var action1 = ActionAsync(); 
            var action2 = ActionAsync();

            Ac1Async = await action1; 
            Ac2Async = await action2; 

            EndAsync = DateTime.Now;

            await InvokeAsync(StateHasChanged); 
        }

        private async Task<string> Action()
        {
            await Await(2000); 
            return "aze";
        }

        private async Task<string> ActionAsync()
        {
            await Await(2000);
            return "aze"; 
        }

        private async Task Await(int duration)
        {
            await Task.Delay(2000);
        }

    }
}
