using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component.Modal.Called
{
    public interface IModalComponentCalled : IComponent
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public string DataId { get; set; }
        public object ReturnedObject { get; set; }

        public void Submit();
        public void Cancel();
    }
}
