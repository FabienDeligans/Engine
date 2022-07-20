using System;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index
    {
        private string Date { get; set; }
        private string Time { get; set; }
        private System.Timers.Timer? Timer { get; set; }

        private int Counter { get; set; }

        protected override void OnInitialized()
        {
            //var timer = new System.Threading.Timer((_) =>
            //{
            //    InvokeAsync(async () =>
            //    {
            //        Date = DateTime.Now.ToLongDateString();
            //        Time = DateTime.Now.ToLongTimeString(); 
                    
            //        await InvokeAsync(StateHasChanged);
            //    });
            //}, null, 0, 500);

            RunLoop(1000);

        }

        public void RunLoop(int interval)
        {
            Timer = new System.Timers.Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = interval,
            };
            Timer.Elapsed += Event;
        }

        private void Event(object? sender, ElapsedEventArgs elapsedEventArgs)
        {
            Date = elapsedEventArgs.SignalTime.ToLongDateString();
            Time = elapsedEventArgs.SignalTime.ToLongTimeString();

            InvokeAsync(StateHasChanged); 
        }

        private void Add()
        {
            Counter++; 
            StateHasChanged();
        }
    }
}
