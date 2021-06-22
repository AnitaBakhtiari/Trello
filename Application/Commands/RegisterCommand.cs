using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Commands
{
    public class RegisterCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
