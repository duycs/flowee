using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Events
{
    public interface IEventDispatcher
    {
        Task RaiseEvent<T>(T @event) where T : DomainEvent;
    }
}
