using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Events
{
    public class NewTaskEvent : INotification
    {

        public string ConectionId { get; set; }


    }
}
