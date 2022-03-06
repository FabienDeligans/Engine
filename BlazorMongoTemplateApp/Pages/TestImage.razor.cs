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
        private string Image { get; set; }
        private string Error { get; set; }


        private async Task OnChange(InputFileChangeEventArgs e)
        {
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

                using var context = ContextFactory.MakeContext();
                context.DropCollection<MyEntity>();

                context.Insert(new MyEntity(){Data = data});

                Image = context.QueryCollection<MyEntity>().FirstOrDefault()?.Data;

            }
            catch (Exception exception)
            {
                Error = exception.Message;
            }

        }
    }
}
