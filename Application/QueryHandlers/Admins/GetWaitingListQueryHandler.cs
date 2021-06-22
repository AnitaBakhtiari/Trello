using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries;
using Infra.Extentions;
using Infra.Models;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.QueryHandlers
{
    class GetWaitingListQueryHandler : IRequestHandler<GetWaitingListQuery, IEnumerable<UserTask>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        public GetWaitingListQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;

        }
        public async Task<IEnumerable<UserTask>> Handle(GetWaitingListQuery request, CancellationToken cancellationToken)
        {
    
            var AdminTask = await _unitOfWork.UserTaskRepository.GetWaitingListTasksAdmin(_accessor.GetUserId());
            return AdminTask;


        }

    }
}
