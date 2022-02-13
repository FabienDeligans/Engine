using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component.Modal.Called
{
    public abstract class ModalComponentCalled : ComponentBase, IModalComponentCalled
    {
        [CascadingParameter]
        public BlazoredModalInstance ModalInstance { get; set; }
        
        [Parameter]
        public string DataId { get; set; }
        
        public object ReturnedObject { get; set; }

        public void Submit()
        {
            ModalInstance.CloseAsync(ModalResult.Ok(ReturnedObject));
        }

        public void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}
