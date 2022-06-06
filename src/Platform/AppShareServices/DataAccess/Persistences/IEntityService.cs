using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess.Persistences
{
    public interface IEntityService
    {
        Guid Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
    }
}
