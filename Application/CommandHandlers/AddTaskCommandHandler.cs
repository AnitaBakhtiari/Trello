using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using AutoMapper;
using Infra.Extentions;
using Infra.Models;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CommandHandlers
{
    class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, int>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AddTaskCommandHandler(IHttpContextAccessor accessor, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _accessor = accessor;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            request.AdminId = _accessor.GetUserId();
            var task = _mapper.Map<UserTask>(request);
            await _unitOfWork.UserTaskRepository.AddTask(task);
            await _unitOfWork.SaveChangeAsync();

            return task.Id;

        }
    }
}
