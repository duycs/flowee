using AppShareServices.Commands;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.Commands;
using WorkerDomain.Events;

namespace WorkerInfrastructure.CrossCuttingIoC
{
    /// <summary>
    /// command, event, domain services
    /// </summary>
    public class DomainInjector
    {
        public static void Register(IServiceCollection services)
        {
            // Mediatr
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<WorkerCreatedEvent>, DomainEventHandler<WorkerCreatedEvent>>();

            // Command
            services.AddScoped<IRequestHandler<CreateWorkerCommand>, WorkerCommandHandler>();
        }
    }
}
