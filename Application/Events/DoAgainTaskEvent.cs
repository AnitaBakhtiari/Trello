using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Events
{
    public class DoAgainTaskEvent : INotification
    {
        public string ConnectionId { get; set; }
    }
}
