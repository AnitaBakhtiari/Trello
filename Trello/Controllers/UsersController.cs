using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.Queries;
using Infra.Extentions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Trello.Controllers
{


    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {

            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult<string>> Register([FromBody] RegisterCommand register)
        {

            return await _mediator.Send(register);
        }


        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginQuery query)
        {
            return await _mediator.Send(query);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Test([FromServices] IHttpContextAccessor accessor)
        {

            return Ok(accessor.GetUserId());


        }

        [HttpPost]
        public async Task<ActionResult<string>> RegisterAdmin([FromBody] RegisterAdminCommand register)
        {

            return await _mediator.Send(register);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
        public IActionResult TestAdmin([FromServices] IHttpContextAccessor accessor)
        {

            return Ok(accessor.GetUserId());

        }



    }
}
