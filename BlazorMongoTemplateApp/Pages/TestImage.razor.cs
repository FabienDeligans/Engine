using System;
using System.IO;
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
        string src2;
        private string path;

        public MyEntity MyNewEntity { get; set; }
        protected override void OnInitialized()
        {
            MyNewEntity = new MyEntity();
        }


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
            Save(e.File);
            //using var stream = e.File.OpenReadStream();
            //using var ms = new MemoryStream();
            //await stream.CopyToAsync(ms);
            //src = "data:" + e.File.ContentType + ";base64," + Convert.ToBase64String(ms.ToArray());
        }

        private void Save(IBrowserFile file)
        {
            using var context = ContextFactory.MakeContext(); 
            context.DropDatabase();
            MyNewEntity.Fichier = file;
            context.Insert(MyNewEntity);

            var fileDownload = context.GetEntity<MyEntity>(MyNewEntity.Id);
            using var ms = new MemoryStream();
            src2 = "data:" + fileDownload.Fichier.ContentType + ";base64," + Convert.ToBase64String(ms.ToArray());
            StateHasChanged();
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
        }
    }
}
