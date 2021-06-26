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
    public class DoTaskByUserEventHandler : INotificationHandler<DoTaskByUserEvent>
    {
        private readonly INotificationHub _hub;
        public DoTaskByUserEventHandler(INotificationHub hub)
        {
            _hub = hub;
        }
        public async Task Handle(DoTaskByUserEvent notification, CancellationToken cancellationToken)
        {
            await _hub.SendMessage(notification.ConnectionId, "This user complete your task");
        }

    }
}
