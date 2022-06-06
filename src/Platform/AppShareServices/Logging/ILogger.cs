using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Logging
{
    public interface ILogger
    {
        public void Info(string message);
        public void Debug(string message);
        public void Error(Exception ex, string message);
    }
}
