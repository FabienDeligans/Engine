using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorMongoTemplateApp.Component.Modal.Called;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component.Modal.Caller
{
    public abstract class ModalComponentCaller : ComponentBase, IModalComponentCaller
    {
        [CascadingParameter]
        public IModalService Modal { get; set; }
        public object DataReturned { get; set; }

        public virtual async Task ShowModal<T>(string dataId, string title) where T : IComponent
        {
            var parameters = new ModalParameters();
            if (dataId != null)
            {
                parameters.Add("DataId", dataId);
            }

            var modal = Modal.Show<T>(title, parameters);
            var result = await modal.Result;

            if (result.Cancelled)
            {
                Modal.Show<OperationCancelled>();
            }
            else
            {
                DataReturned = result.Data;
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
