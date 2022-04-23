using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Models;
using MongoDB.Bson;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private void Do()
        {
            using var process = new Process();
            process.StartInfo.FileName = $@"C:\Program Files\Notepad++\notepad++.exe";
            process.Start();
        }

        private decimal ProgressPercent { get; set; }

        private int I { get; set; }
        private async Task Do2()
        {
            var nb = 100;
            I = 0; 
            while (I < nb)
            {
                await Task.Delay(100);
                I++;
                StateHasChanged(); ProgressPercent = Decimal.Divide(I, nb);
            }

        }

        private void Do3()
        {
            I++; 
        }
    }
}
