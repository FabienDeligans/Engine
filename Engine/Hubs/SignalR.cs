using System.Collections.Generic;
using System.Threading.Tasks;
using Engine.Database;
using Engine.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Engine.Hubs
{
    public abstract class SignalR<T> : ComponentBase, ISignalR
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected HubConnection DataHubConnection { get; set; }
        protected T Data { get; set; }
        protected List<T> DataList { get; set; } = new List<T>();
        public abstract BaseContext Context { get; set; }

        public abstract Task InitSignalR();
        public abstract Task InitData();

        public async Task RefreshSignalR(object data, Crud crud)
        {
            await DataHubConnection.SendAsync(DataHubMethod.SendData.GetName(), data, crud);
        }

        public bool IsConnected => DataHubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (DataHubConnection is not null)
            {
                await DataHubConnection.DisposeAsync();
            }
        }
    }
}
