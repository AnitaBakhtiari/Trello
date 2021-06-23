using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Trello.Hubs
{
    public class NotificationHub: Hub
    {
        public async Task SendMessage(string Id , string Message)
        {
            await Clients.Group("Id").SendAsync("Alert", Message);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


    }
}
