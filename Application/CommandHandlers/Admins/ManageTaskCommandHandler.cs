using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.Admins;
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
        private readonly NotificationHub _hub;

        public ManageTaskCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(ManageTaskCommand request, CancellationToken cancellationToken)
        {

            var task = _unitOfWork.UserTaskRepository.ManageTask(request.Id, _accessor.GetUserId(), request.Status);
            await _unitOfWork.SaveChangeAsync();
            if (request.Status == "DoAgain")
            {
                await _hub.SendMessage(_accessor.GetUserId(), "Please Complete this Task");
            }

            return request.Id;

        }
    }
}
