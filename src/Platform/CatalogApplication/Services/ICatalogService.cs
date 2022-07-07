using CatalogApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApplication.Services
{
    public interface ICatalogService
    {
        Task<CatalogDto> Get(int id, bool isInclude);
    }
}
