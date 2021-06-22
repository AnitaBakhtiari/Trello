using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries;
using Infra.Models;
using Infra.Repositories;
using MediatR;

namespace Application.QueryHandlers
{
    public class GetListQueryHandler : IRequestHandler<GetListQuery, IEnumerable<UserTask>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<IEnumerable<UserTask>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserTaskRepository.GetListTasks();
        }
    }
}
