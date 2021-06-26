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
    class ToDoWorkerEventHandler : INotificationHandler<ToDoWorkerEvent>
    {
        private readonly INotificationHub _hub;

        public ToDoWorkerEventHandler(INotificationHub hub)
        {
            _hub = hub;
        }
        public async Task Handle(ToDoWorkerEvent notification, CancellationToken cancellationToken)
        {
            await _hub.SendMessage(notification.ConectionId, "You have had new task, please check it ");
        }


    }
}
