using AppShareDomain.Models;
using AppShareServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AppShareServices.Pagging
{
    public class ColumnOrder
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Order Order { get; set; }

        public ColumnOrder(string name, Order order)
        {
            Name = name;
            Order = order;
        }
    }
}
