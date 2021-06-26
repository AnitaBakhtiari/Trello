using System;
using System.Threading.Tasks;
using Application.Hubs;
using Infra.Extentions;
using Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Trello.Hubs
{


    public class NotificationHub : Hub , INotificationHub
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IUnitOfWork _unitOfWork;


        public NotificationHub(IHttpContextAccessor accessor, IUnitOfWork unitOfWork)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
        }


     
        public async Task SendMessage(string id, string message)
        {
            await Clients.Client(id).SendAsync("Alert", message);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override Task OnConnectedAsync()
        {
            var id = _accessor.GetUserId();
            var connectionId = Context.ConnectionId;
            _unitOfWork.UserRepository.UpdateSignalR(id, connectionId);
            _unitOfWork.SaveChangeAsync();

            return base.OnConnectedAsync();
        }



        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


    }
}
