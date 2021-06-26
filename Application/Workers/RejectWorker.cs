using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Events;
using Application.Hubs;
using Infra.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Workers
{
    public class RejectWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public RejectWorker(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30));
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var ToDoTasks = await unitOfWork.UserTaskRepository.GetListToDoTasks();

                        foreach (var item in ToDoTasks)
                        {
                            var connectionId = await unitOfWork.UserRepository.FindConnectionIdAsync(item.UserId);

                            if (DateTime.Now >= item.Date.AddHours(1) && DateTime.Now <= item.Date.AddHours(1).AddSeconds(30))
                            {
                                await _mediator.Publish(new ReminderTaskEvent() { ConectionId = connectionId });
                            }



                            if (DateTime.Now > item.Date)
                            {
                                item.Status = "ToDo";
                                item.Date = item.Date.AddDays(10);
                                await _mediator.Publish(new ToDoWorkerEvent() { ConectionId = connectionId });
                            }
                        }

                        await unitOfWork.SaveChangeAsync();

                    }
                    catch (Exception)
                    {


                    }
                }



            }
        }
    }
}

