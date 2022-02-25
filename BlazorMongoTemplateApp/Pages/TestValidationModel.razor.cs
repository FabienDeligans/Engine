using System;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TestValidationModel
    {
        private string _statusMessage;
        private string _statusClass;
        private readonly MyEntity _model = new MyEntity();

        private void ValidSubmit()
        {
            _statusClass = "alert-info";
            _statusMessage = DateTime.Now + " VALID";

            using var context = ContextFactory.MakeContext();
            try
            {
                context.Insert(_model);
            }
            catch (Exception e)
            {
                _statusClass = "alert-danger";
                _statusMessage = e.Message;
            }
            StateHasChanged();
        }

        private void InvalidSubmit()
        {
            _statusClass = "alert-danger";
            _statusMessage = DateTime.Now + " INVALID";
        }
    }
}
