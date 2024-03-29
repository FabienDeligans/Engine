﻿using System;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Database;
using Engine.Model;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TestImage
    {
        private string Error { get; set; } = "";
        private Fichier Fichier { get; set; }
        protected override void OnInitialized()
        {
            using var context = ContextFactory.MakeContext();
            context.DropDatabase();
            Fichier = new Fichier();
        }

        // Called when a new file is uploaded
        private async Task UploadFileIntoDatabaseAsync(InputFileChangeEventArgs e)
        {
            Error = "";
            try
            {
                using var context = ContextFactory.MakeContext();
                var file = context.UploadFileAsync(e.File);

                WaitUpload();

                Fichier = await file;
                Id = Fichier.Id;
                Fichier = new Fichier();

                Error = "";
            }

            catch (Exception exception)
            {
                Error = exception.Message;
            }
            await InvokeAsync(StateHasChanged);
        }

        private void WaitUpload()
        {
            Error = "Uploading in progress " + DateTime.Now.ToLongTimeString();
            StateHasChanged();
        }

        private string Id { get; set; }

        private async Task DownloadPictureFromDatabaseAsync(string id)
        {
            Error = "";
            try
            {
                using var context = ContextFactory.MakeContext();
                var fichier = context.DownloadFileAsync(id);

                WaitDownload();

                Fichier = await fichier;

                Error = "";
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            await InvokeAsync(StateHasChanged);
        }

        private void WaitDownload()
        {
            Error = "Downloading in progress " + DateTime.Now.ToLongTimeString();
            StateHasChanged();
        }
    }
}
