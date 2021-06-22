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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserTaskController : Controller
    {

        private readonly IMediator _mediator;
        public UserTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public Task<IEnumerable<UserTask>> GetArchiveList(GetArchiveListQuery getList)
        {
            return _mediator.Send(getList);
        }


        [HttpPost]
        public Task<int> DoTask(int Id)
        {
            return _mediator.Send(new DoTaskCommand() { Id=Id});
        }

    }
}
