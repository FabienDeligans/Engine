using System.Diagnostics;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index 
    {
        private void Do()
        {
            var process = new Process();
            process.StartInfo.FileName = $@"C:\Program Files\Notepad++\notepad++.exe";
            process.Start();
        }
    }
}
