using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Pagging
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilterOrder filter, string route);
    }
}
