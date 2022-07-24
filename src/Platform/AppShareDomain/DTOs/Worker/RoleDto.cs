using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareDomain.DTOs.Worker
{
    public class RoleDto : DtoBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
