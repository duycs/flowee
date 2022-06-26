using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace AppShareServices.Models
{
    public class PaginationFilterOrder
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// eg: FullName_DESC,Code_ASC,Email_ASC
        /// </summary>
        public string ColumnOrders { get; set; }

        public PaginationFilterOrder()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.ColumnOrders = "";
        }

        public PaginationFilterOrder(int pageNumber, int pageSize, string? columnOrders = "")
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.ColumnOrders = columnOrders ?? "";
        }
    }
}
