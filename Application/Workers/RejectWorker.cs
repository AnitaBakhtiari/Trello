using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Workers
{
    public class RejectWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RejectWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                        var TaskList = await unitOfWork.UserTaskRepository.GetListTasks();
                        var ToDoTasks = TaskList.Where(a => a.Status == "ToDo");
                        foreach (var item in ToDoTasks)
                        {
                            if (DateTime.Now > item.Date)
                            {
                                item.Status = "Reject";
                                item.Date = item.Date.AddDays(10);
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
