using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Commands
{
    public class RegisterAdminCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
