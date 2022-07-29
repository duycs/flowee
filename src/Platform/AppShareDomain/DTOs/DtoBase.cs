using System.Text.Json.Serialization;

namespace AppShareDomain.DTOs
{
    public class DtoBase
    {
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime DateCreated { get; set; }
        [JsonIgnore]
        public DateTime DateModified { get; set; }
    }
}
