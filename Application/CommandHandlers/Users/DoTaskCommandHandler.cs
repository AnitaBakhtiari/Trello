using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Application.Events;
using Infra.Data;
using Infra.Extentions;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Trello.Hubs;

namespace Application.CommandHandlers
{
    class DoTaskCommandHandler : IRequestHandler<DoTaskCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMediator _mediator;
  

        public DoTaskCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor, IMediator mediator, ApplicationDbContext context)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(DoTaskCommand request, CancellationToken cancellationToken)
        {

            //TODO
            var waiting = await _unitOfWork.UserTaskRepository.DoTask(request.Id, _accessor.GetUserId());
            await _unitOfWork.SaveChangeAsync();
             var connectionId = await _unitOfWork.UserRepository.FindConnectionIdAsync(waiting.UserId);
            await _mediator.Publish(new DoTaskByUserEvent() { ConnectionId = connectionId });

            return request.Id;
        }
    }
}
