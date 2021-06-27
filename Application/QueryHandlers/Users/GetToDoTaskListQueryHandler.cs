using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.Users;
using Infra.Extentions;
using Infra.Models;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.QueryHandlers.Users
{
   public class GetToDoTaskListQueryHandler : IRequestHandler<GetToDoTaskListQuery, IEnumerable<UserTask>>
    {
        private readonly IUnitOfWork _unitOfWrok;
        private readonly IHttpContextAccessor _accessor;

        public GetToDoTaskListQueryHandler(IUnitOfWork unitOfWork,IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _unitOfWrok = unitOfWork;
        }


        public async Task<IEnumerable<UserTask>> Handle(GetToDoTaskListQuery request, CancellationToken cancellationToken)
        {
           var userId = _accessor.GetUserId();

           await _unitOfWrok.UserTaskRepository.GetListToDoTasksByUser(userId);







            throw new NotImplementedException();
        }
    }
}
