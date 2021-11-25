using System.ComponentModel;
using System.Threading.Tasks;
using Engine.Enum;
using Microsoft.AspNetCore.SignalR;

namespace Engine.Hubs
{
    public class DataHub : Hub
    {
        public async Task SendData(object data, Crud crud)
        {
            await Clients.All.SendAsync(DataHubMethod.ReceiveData.GetName(), data, crud);
        }
    }


    public enum DataHubMethod
    {
        [Description("ReceiveData")]
        ReceiveData,

        [Description("SendData")]
        SendData,

    }

    public enum Crud
    {
        Create,
        Read,
        Update,
        Delete
    }
}
