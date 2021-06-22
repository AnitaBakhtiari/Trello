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
    public class GetArchiveListQueryHandler : IRequestHandler<GetArchiveListQuery, IEnumerable<UserTask>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        public GetArchiveListQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;

        }
        public async Task<IEnumerable<UserTask>> Handle(GetArchiveListQuery request, CancellationToken cancellationToken)
        {
            var admin = await _unitOfWork.UserTaskRepository.GetListTasks();
            var result = admin.Where(a => a.AdminId == null);

            if (result == null)
            {
                return await _unitOfWork.UserTaskRepository.GetListArchiveTasks(_accessor.GetUserId());
            }
            else
                return await _unitOfWork.UserTaskRepository.GetListArchiveTasksAdmin(_accessor.GetUserId());






        }
    }
}
