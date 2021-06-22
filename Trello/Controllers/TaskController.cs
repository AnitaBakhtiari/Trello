using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.Queries;
using Infra.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Trello.Controllers
{
    public class TaskController : Controller
    {
        private readonly IMediator _mediator;
        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public Task<IEnumerable<UserTask>> GetList(GetListQuery getList)
        {
            return _mediator.Send(getList);
        }


        [HttpGet]
        public Task<IEnumerable<UserTask>> GetArchiveList(GetArchiveListQuery getList)
        {
            return _mediator.Send(getList);
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public Task<int> AddCategory(AddCategoryCommand categoryName)
        {
            return _mediator.Send(categoryName);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public Task<int> AddTask(AddTaskCommand taskName )
        {
            return _mediator.Send(taskName);
        }






    }
}
