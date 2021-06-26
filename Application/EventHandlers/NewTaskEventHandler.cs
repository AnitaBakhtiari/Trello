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
    public class NewTaskEventHandler : INotificationHandler<NewTaskEvent>
    {
        private readonly INotificationHub _hub;

        public NewTaskEventHandler(INotificationHub hub)
        {
            _hub = hub;
        }
        public async Task Handle(NewTaskEvent notification, CancellationToken cancellationToken)
        {
            await _hub.SendMessage(notification.ConectionId, "You have new task ");
        }
    }
}
