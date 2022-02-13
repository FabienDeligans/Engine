using System.Threading.Tasks;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Component.Modal.Caller
{
    public interface IModalComponentCaller
    {
        [CascadingParameter] public IModalService Modal { get; set; }
        Task ShowModal<T>(string dataId) where T : IComponent;
    }
}