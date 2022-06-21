using AppShareServices.Commands;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
            services.AddMediatR(Assembly.GetExecutingAssembly());
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
