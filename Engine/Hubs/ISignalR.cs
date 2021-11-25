using System;
using System.Threading.Tasks;
using Engine.Database;
using Microsoft.AspNetCore.Components;

namespace Engine.Hubs
{
    public interface ISignalR : IComponent, IAsyncDisposable
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        Task InitSignalR();
        Task InitData();
        Task RefreshSignalR(object data, Crud crud);
        BaseContext Context { get; set; }

    }
}