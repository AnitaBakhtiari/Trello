using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using AutoMapper;
using Infra.Models;
using Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwt;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwt, IMapper mapper)
        {
            _userManager = userManager;
            _jwt = jwt;
            _mapper = mapper;
        }
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //var user = new ApplicationUser()
            //{
            //    UserName = request.Email,
            //    Email = request.Email,
            //};

            var user = _mapper.Map<ApplicationUser>(request);

            await _userManager.CreateAsync(user, request.Password);

            return await _jwt.GetTokenAsync(user);



        }
    }
}
