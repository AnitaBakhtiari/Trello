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
    class GetArchiveListByAdminQueryHandler : IRequestHandler<GetArchiveListByAdminQuery, IEnumerable<UserTask>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        public GetArchiveListByAdminQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;

        }
        public async Task<IEnumerable<UserTask>> Handle(GetArchiveListByAdminQuery request, CancellationToken cancellationToken)
        {
          //  request.AdminId =  _accessor.GetUserId();

            var AdminTask= await _unitOfWork.UserTaskRepository.GetListArchiveTasksAdmin(_accessor.GetUserId());
            return AdminTask;


        }

    }
}
