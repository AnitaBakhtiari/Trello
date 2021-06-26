using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Events;
using Application.Hubs;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Workers
{
    public class RejectWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationHub _hub;

        public RejectWorker(IServiceProvider serviceProvider, INotificationHub hub)
        {
            _serviceProvider = serviceProvider;
            _hub = hub;
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(TimeSpan.FromSeconds(10));

                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {

                        
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var ToDoTasks = await unitOfWork.UserTaskRepository.GetListToDoTasks();
                  

                        foreach (var item in ToDoTasks)
                        {
                            if (DateTime.Now > item.Date)
                            {
                                item.Status = "ToDo";
                                item.Date = item.Date.AddDays(10);
                                var connectionId = await unitOfWork.UserRepository.FindConnectionIdAsync(item.UserId);
                                await _hub.SendMessage(new ToDoWorkerEvent() { ConectionId = connectionId });

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
