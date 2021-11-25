using System;
using System.Linq;
using System.Threading.Tasks;
using Engine.Database;
using Engine.Enum;
using Engine.Model;
using Microsoft.AspNetCore.SignalR.Client;

namespace Engine.Hubs
{
    public abstract class SignalRDataList<T> : SignalR<T> where T : IEntity
    {
        public override BaseContext Context { get; set; }
        public override async Task InitSignalR()
        {
            using var context = Context;
            DataList = context.QueryCollection<T>().ToList();

            await InitData();
        }

        public override async Task InitData()
        {
            DataHubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/dataHub"))
                .Build();

            DataHubConnection.On<T, Crud>(DataHubMethod.ReceiveData.GetName(), (data, crud) =>
            {
                Data = data;

                switch (crud)
                {
                    case Crud.Create:
                    {
                        DataList.Add(Data);
                        break;
                    }
                    case Crud.Read:
                    {
                        using var context = Context;
                        DataList = context.QueryCollection<T>().ToList();
                        break;
                    }
                    case Crud.Update:
                    {
                        var index = DataList.FindIndex(v => v.Id == Data.Id);
                        DataList[index] = Data;
                        break;
                    }
                    case Crud.Delete:
                    {
                        var index = DataList.FindIndex(v => v.Id == Data.Id);
                        DataList.RemoveAt(index);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(crud), crud, null);
                }
                StateHasChanged();
            });
            await DataHubConnection.StartAsync();
        }
    }
}
