using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Commands.Admins
{
    public class ManageTaskCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Status { get; set; }

    }
}
