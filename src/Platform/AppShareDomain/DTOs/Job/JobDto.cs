using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppShareDomain.DTOs.Operation;

namespace AppShareDomain.DTOs.Job
{
    public class JobDto : DtoBase
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public List<OperationDto> Operations { get; set; }
        public int JobStatusId { get; set; }
        public EnumerationDto JobStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
