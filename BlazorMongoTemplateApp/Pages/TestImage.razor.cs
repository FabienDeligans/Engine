using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Database;
using Engine.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MongoDB.Bson;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TestImage
    {
        private FileMax512 Image { get; set; }
        private string Error { get; set; } = "";

        protected override void OnInitialized()
        {
            Image = new FileMax512();
            FileMax512 = new FileMax512();
        }

        private async Task OnChange(InputFileChangeEventArgs e)
        {
            Error = "";

            try
            {
                if (e.File.Size >= 512000)
                {
                    throw new Exception("Fichier trop volumineux");
                }

                await using var stream = e.File.OpenReadStream();
                await using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var data = "data:" + e.File.ContentType + ";base64," + Convert.ToBase64String(ms.ToArray());

                var fichier = new FileMax512
                {
                    Name = e.File.Name, 
                    Type = e.File.ContentType, 
                    Size = e.File.Size, 
                    Data = data, 
                    CreationDate = DateTime.Now
                }; 

                using var context = ContextFactory.MakeContext();
                context.DropDatabase();

                context.Insert(fichier);

                Image = context.QueryCollection<FileMax512>().LastOrDefault();

            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }

        }


        ElementReference dropZoneElement;
        InputFile inputFile;

        IJSObjectReference _module;
        IJSObjectReference _dropZoneInstance;

        public FileMax512 FileMax512 { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Load the JS file
                _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./dropZone.js");

                // Initialize the drop zone
                _dropZoneInstance = await _module.InvokeAsync<IJSObjectReference>("initializeFileDropZone", dropZoneElement, inputFile.Element);
            }
        }

        // Called when a new file is uploaded
        private async Task OnDragAndDrop(InputFileChangeEventArgs e)
        {
            Error = ""; 
            try
            {
                using var stream = e.File.OpenReadStream(99999999999);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var data = "data:" + e.File.ContentType + ";base64," + Convert.ToBase64String(ms.ToArray());

                using var context = ContextFactory.MakeContext();

                byte[] buffer = new byte[16 * 1024];
                int read;

                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                var arrayByte = ms.ToArray(); 

                Id = await context.UploadFile(e.File.Name, arrayByte);

            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }

        }
        private ObjectId Id { get; set; }

        private async void Do()
        {
            using var context = ContextFactory.MakeContext();

            var (FileName, file) = await context.DownloadFile(Id);
            var ext = Path.GetExtension(FileName); 

            var truc = Convert.ToBase64String(file);
            FileUrl = string.Format("data:" + ext, truc); 
        }

        private string FileUrl; 
        private string FileName; 

        // Unregister the drop zone events
        public async ValueTask DisposeAsync()
        {
            if (_dropZoneInstance != null)
            {
                await _dropZoneInstance.InvokeVoidAsync("dispose");
                await _dropZoneInstance.DisposeAsync();
            }

            if (_module != null)
            {
                await _module.DisposeAsync();
            }
        }

    }
}
