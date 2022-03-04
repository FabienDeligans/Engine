using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TestImage
    {
        ElementReference dropZoneElement;
        InputFile inputFile;

        IJSObjectReference _module;
        IJSObjectReference _dropZoneInstance;

        string src;
        public string Error { get; set; }

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
        async Task OnChange(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.File.Size >= 512000)
                {
                    throw new Exception("Fichier trop volumineux");
                }

                using var stream = e.File.OpenReadStream();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var file = "data:" + e.File.ContentType + ";base64," + Convert.ToBase64String(ms.ToArray());

                using var context = ContextFactory.MakeContext();
                context.DropCollection<Picture>();
                context.Insert(
                    new Picture
                    {
                        File = file,
                        Name = e.File.Name
                    });

                src = context.QueryCollection<Picture>().FirstOrDefault()?.File;

            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }
            finally
            {
                await _module.DisposeAsync();
            }

        }


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

            Error = ""; 
        }
    }
}
