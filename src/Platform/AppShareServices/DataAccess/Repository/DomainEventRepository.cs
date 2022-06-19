using AppShareServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess.Repository
{
    public class DomainEventRepository : IDomainEventRepository
    {
        private readonly EventContext _eventContext;

        public DomainEventRepository(EventContext eventContext)
        {
            _eventContext = eventContext;
        }

        public void Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
        {
            _eventContext.DomainEventRecords.Add(new DomainEventRecord()
            {
                Created = domainEvent.Created,
                Type = domainEvent.Type,
                Content = domainEvent.Content,
                CorrelationId = domainEvent.CorrelationId
            });
            _eventContext.SaveChanges();
        }
    }
}
