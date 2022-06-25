using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Pagging
{
    public static class QueriesExtensions
    {
        public static List<ColumnOrder> ToColumnOrders(this string queriesString)
        {
            if (string.IsNullOrEmpty(queriesString))
                return new List<ColumnOrder>();

            var columnOrders = new List<ColumnOrder>();
            var queries = queriesString.Split(',');

            // only 1 column order
            if (!queries.Any())
            {
                var items = queriesString.Split("_");
                if (items.Any() && items.Length == 2)
                {
                    var order = (Order)Enum.Parse(typeof(Order), items[1], true);
                    var columnOrder = new ColumnOrder(items[0], order);
                    columnOrders.Add(columnOrder);
                }
            }
            else
            {
                // many column order
                foreach (var query in queries)
                {
                    var items = query.Split("_");
                    if (items.Any() && items.Length == 2)
                    {
                        var order = (Order)Enum.Parse(typeof(Order), items[1], true);
                        var columnOrder = new ColumnOrder(items[0], order);
                        columnOrders.Add(columnOrder);
                    }
                }
            }

            return columnOrders;
        }
    }
}
