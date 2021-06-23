using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.Commands.Admins;
using Application.Queries;
using Infra.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Trello.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminTaskController : BaseController
    {
        private readonly IMediator _mediator;
        public AdminTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public Task<IEnumerable<UserTask>> GetList(GetListQuery getList)
        {
            return _mediator.Send(getList);
        }


        [HttpGet]
        public Task<IEnumerable<UserTask>> GetArchiveListByAdmin(GetArchiveListByAdminQuery getList)
        {
            return _mediator.Send(getList);
        }

        [HttpGet]
        public Task<IEnumerable<UserTask>> GetWaitingListByAdmin(GetWaitingListQuery getList)
        {
            return _mediator.Send(getList);
        }


        [HttpPost]
        public Task<int> AddCategory(AddCategoryCommand categoryName)
        {
            return _mediator.Send(categoryName);
        }


        [HttpPost]
        public Task<int> AddTask(AddTaskCommand taskName)
        {
            return _mediator.Send(taskName);
        }

        [HttpPost]
        public Task<int> ManageTask(ManageTaskCommand query)
        {
            return _mediator.Send(query);
        }


    }
}
