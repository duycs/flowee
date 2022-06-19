using AppShareServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess.Repository
{
    public interface IDomainEventRepository
    {
        void Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;
    }
}
