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

namespace Application.CommandHandlers.Admins
{
    class DoAgainTaskCommandHandler : IRequestHandler<DoAgainTaskCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;

        public DoAgainTaskCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DoAgainTaskCommand request, CancellationToken cancellationToken)
        {

            await _unitOfWork.UserTaskRepository.DoAgainTask(request.Id, _accessor.GetUserId());
            await _unitOfWork.SaveChangeAsync();
            return request.Id;

        }
    }
}
