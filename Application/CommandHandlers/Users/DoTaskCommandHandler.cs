using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Infra.Extentions;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CommandHandlers
{
    class DoTaskCommandHandler : IRequestHandler<DoTaskCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;

        public DoTaskCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DoTaskCommand request, CancellationToken cancellationToken)
        {
            var waiting = _unitOfWork.UserTaskRepository.DoTask(request.Id, _accessor.GetUserId());
            if (waiting.AsyncState == null)
            {
                return 404;
            }
            else
                await _unitOfWork.SaveChangeAsync();


            return request.Id;
        }
    }
}
