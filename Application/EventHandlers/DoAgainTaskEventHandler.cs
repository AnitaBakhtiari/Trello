using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Events;
using Application.Hubs;
using MediatR;

namespace Application.EventHandlers
{
    public class DoAgainTaskEventHandler : INotificationHandler<DoAgainTaskEvent>
    {
        private readonly INotificationHub _hub;
        public DoAgainTaskEventHandler(INotificationHub hub)
        {
            _hub = hub;
        }
        public async Task Handle(DoAgainTaskEvent notification, CancellationToken cancellationToken)
        {
            await _hub.SendMessage(notification.ConnectionId, "Do this task again");
        }
    }
}
