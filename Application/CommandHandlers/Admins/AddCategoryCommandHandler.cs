using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using AutoMapper;
using Infra.Models;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CommandHandlers
{
    class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _mapper = mapper;


        }
        public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            await _unitOfWork.CategoryRepository.AddCategory(category);
            await _unitOfWork.SaveChangeAsync();
            return category.Id;
        }
    }
}
