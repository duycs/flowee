using AppShareServices.Models;

namespace CatalogApplication.DTOs
{
    public class SpecificationDto : DtoBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SettingDto>? Settings { get; set; }
    }
}
