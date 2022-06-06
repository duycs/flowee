using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Models
{
    public class ColumnOrder
    {
        public int Column { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Order Order { get; set; }
    }
}
