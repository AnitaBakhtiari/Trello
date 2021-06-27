using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries;
using Infra.Data;
using Infra.Models;
using Infra.Repositories;
using Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.QueryHandlers
{
    public class LoginQueryhandler : IRequestHandler<LoginQuery, string>
    {
          
        private readonly SignInManager<ApplicationUser> _sign;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwt;

        public LoginQueryhandler(SignInManager<ApplicationUser> sign, IJwtService jwt, IUnitOfWork unitOfWork)
        {
            _sign = sign;
            _jwt = jwt;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FindByEmailAsync(request.Email);
            // var user = await _context.ApplicationUsers.SingleOrDefaultAsync(a => a.Email == request.Email);
            var result = await _sign.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                return string.Empty;
            }

            return await _jwt.GetTokenAsync(user);





        }
    }
}
