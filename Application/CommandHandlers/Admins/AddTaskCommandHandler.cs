using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Application.Events;
using AutoMapper;
using Infra.Data;
using Infra.Extentions;
using Infra.Models;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Trello.Hubs;

namespace Application.CommandHandlers
{
    class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, int>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

       
        public AddTaskCommandHandler(IHttpContextAccessor accessor, IMapper mapper, IUnitOfWork unitOfWork,IMediator mediator)
        {
            _accessor = accessor;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

      
            _mediator = mediator;
        }

        public async Task<int> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
          //  request.AdminId = _accessor.GetUserId();

            var task = _mapper.Map<UserTask>(request);
            task.AdminId =  _accessor.GetUserId();
            await _unitOfWork.UserTaskRepository.AddTask(task);         
            await _unitOfWork.SaveChangeAsync();
            var connectionId = await _unitOfWork.UserRepository.FindConnectionIdAsync(task.UserId);         
          //  await _mediator.Publish(new NewTaskEvent() { ConectionId= connectionId });
            return task.Id;

        }
    }
}
