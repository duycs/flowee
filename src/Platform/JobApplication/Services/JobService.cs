using JobDomain.AgreegateModels.JobAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Services
{
    public class JobService : IJobService
    {
        private readonly ICatalogClientService _catalogClientService;

        public JobService(ICatalogClientService catalogClientService)
        {
            _catalogClientService = catalogClientService;
        }

        public async Task<List<Step>> GenerateSteps(int productId)
        {
            var catalogId = productId;
            var catalogDto = await _catalogClientService.Get(catalogId, true);

            if(catalogDto is not null)
            {

            }
        }
    }
}
