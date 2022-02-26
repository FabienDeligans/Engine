using System;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TestValidationModel
    {
        private string _statusMessage;
        private string _statusClass;
        private readonly MyEntity _model = new MyEntity{Now = DateTime.Now.ToLocalTime()};

        private void Valid()
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

        private void Cancel()
        {
            _statusClass = "alert-danger";
            _statusMessage = DateTime.Now + " INVALID";
        }
    }
}
