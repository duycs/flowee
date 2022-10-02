using AppShareDomain.DTOs.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareApplication.Services
{
    public interface IJobClientService
    {
        public StepDto GetStep(int stepId);
        public void GoToStep(int stepId, string comment);
    }
}
