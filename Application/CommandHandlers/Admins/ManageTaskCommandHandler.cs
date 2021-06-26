using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.Admins;
using Application.Events;
using Infra.Extentions;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Trello.Hubs;

namespace Application.CommandHandlers.Admins
{
    class ManageTaskCommandHandler : IRequestHandler<ManageTaskCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMediator _mediator;
        private readonly NotificationHub _hub;

        public ManageTaskCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor, NotificationHub hub, IMediator mediator)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
            _hub = hub;
            _mediator = mediator;
        }

        public async Task<int> Handle(ManageTaskCommand request, CancellationToken cancellationToken)
        {

            var task = await _unitOfWork.UserTaskRepository.ManageTask(request.Id, _accessor.GetUserId(), request.Status);
            var connectionId = await _unitOfWork.UserRepository.FindConnectionIdAsync(task.UserId);
            if (request.Status == "DoAgain")
            {
                task.Status = "DoAgain";
                task.Date = task.Date.AddDays(10);
                await _mediator.Publish(new DoAgainTaskEvent() { ConnectionId = connectionId });
            }
            else
            {
                task.Status = "Done";
            }

            await _unitOfWork.SaveChangeAsync();

            return request.Id;

        }
    }
}
