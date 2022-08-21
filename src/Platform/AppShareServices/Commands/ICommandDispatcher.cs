using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Commands
{
    public interface ICommandDispatcher
    {
        Task Send<T>(T command) where T : Command;
        Task<string> SendGetResponse<T>(T command) where T : CommandResponse<string>;
    }
}
