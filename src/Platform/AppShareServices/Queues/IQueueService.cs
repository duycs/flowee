using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Queues
{
    public interface IQueueService
    {
        void Send(string url, string message);
        void Send(string url, List<string> message);
    }
}
