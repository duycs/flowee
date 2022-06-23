using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Mappings
{
    public interface IMappingService
    {
        TDestination Map<TDestination>(object source);
    }
}
