using AppShareServices.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess
{
    public class DomainEventEntityTypeConfiguration : IEntityTypeConfiguration<DomainEventRecord>
    {
        public void Configure(EntityTypeBuilder<DomainEventRecord> configuration)
        {
            configuration.ToTable("DomainEventRecords");
            configuration.HasKey(b => b.CorrelationId);
        }
    }
}