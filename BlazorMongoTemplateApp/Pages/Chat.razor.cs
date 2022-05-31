using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;
using Engine.Hubs;
using Microsoft.AspNetCore.Components;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class Chat
    {
        private User User { get; set; }
        private string _userName;
        private string _message;
        private string _toUserId;

        private List<User>UserList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Context = ContextFactory.MakeContext();
            await InitSignalR();
        }

        private void Login()
        {
            User = new User()
            {
                UserName = _userName
            };
            using var context = ContextFactory.MakeContext();

            if (context.QueryCollection<User>().FirstOrDefault(v => v.UserName == _userName) != null)
            {
                User = context.QueryCollection<User>().FirstOrDefault(v => v.UserName == User.UserName);
            }
            else
            {
                context.Insert(User);
                User = context.GetEntity<User>(User.Id);
            }

            UserList = context.QueryCollection<User>().ToList(); 

            StateHasChanged();
        }

        private async Task SendMessage()
        {
            var message = new ChatMessage()
            {
                FromUserId = User.Id,
                ToUserId = _toUserId,
                Message = _message,
                DateTime = DateTime.Now
            };
            using var context = ContextFactory.MakeContext(); 
            context.Insert(message);
            await RefreshSignalR(message, Crud.Create);
            _message = ""; 
        }

        private void SelectToUser(ChangeEventArgs obj)
        {
            _toUserId = obj.Value.ToString(); 
        }
    }
}
