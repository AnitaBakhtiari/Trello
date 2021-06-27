using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Models;
using MediatR;

namespace Application.Queries.Users
{

  public  class GetToDoTaskListQuery : IRequest<IEnumerable<UserTask>>
    {

    }
}
