using System;
using System.Diagnostics;
using System.Text;
using MongoDB.Bson;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index
    {
        private void Do()
        {
            using var process = new Process();
            process.StartInfo.FileName = $@"C:\Program Files\Notepad++\notepad++.exe";
            process.Start();
        }

        private void Do2()
        {
            var stringToConvert = "test";

            var byteArray = new byte[12];

            byteArray = Encoding.Default.GetBytes(stringToConvert);

            var result = BitConverter.ToString(byteArray);

            Id = new ObjectId(byteArray);

            StateHasChanged();
        }

        private ObjectId Id { get; set; }
    }
}
