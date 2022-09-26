using AppShareServices.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationApplication.Commands
{
    public class DownloadFileOperationCommand : Command
    {
        public string FileUrl { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(FileUrl);
        }
    }
}
