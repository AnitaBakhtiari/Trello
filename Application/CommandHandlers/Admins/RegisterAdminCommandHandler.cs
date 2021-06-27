using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using AutoMapper;
using Infra.Data;
using Infra.Models;
using Infra.Repositories;
using Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CommandHandlers
{
    class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwt;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterAdminCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwt, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwt = jwt;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
        {
            //var user = new ApplicationUser()
            //{
            //    UserName = request.Email,
            //    Email = request.Email,
            //};

            var user = _mapper.Map<ApplicationUser>(request);

            // var transaction = await _context.Database.BeginTransactionAsync();

            await _unitOfWork.BeginTransactionAsync();

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                return string.Empty;
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "Admin");

            if (!addRoleResult.Succeeded)
            {
                //  transaction.Rollback();
                await _unitOfWork.RollBackAsync();
                return string.Empty;
            }

            // await transaction.CommitAsync();
            await _unitOfWork.CommitAsync();

            return await _jwt.GetTokenAsync(user);



        }
    }
}

