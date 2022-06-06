using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerInfrastructure.DataAccess.EntityConfigurations
{
    public class WorkerEntityConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            throw new NotImplementedException();
        }
    }
}
