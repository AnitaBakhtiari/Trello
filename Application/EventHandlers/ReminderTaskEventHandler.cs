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
    class ReminderTaskEventHandler : INotificationHandler<ReminderTaskEvent>
    {
        private readonly INotificationHub _hub;

        public ReminderTaskEventHandler(INotificationHub hub)
        {
            _hub = hub;
        }

        public async Task Handle(ReminderTaskEvent notification, CancellationToken cancellationToken)
        {
            await _hub.SendMessage(notification.ConectionId, "Do not forget to do your task ");
        }

     
    }
}
