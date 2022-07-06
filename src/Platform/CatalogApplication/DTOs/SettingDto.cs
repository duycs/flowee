using AppShareServices.Models;

namespace CatalogApplication.DTOs
{
    public class SettingDto : DtoBase
    {
        public string SettingType { get; set; }
        public int Key { get; set; }
        public string Value { get; set; }
        public int Name { get; set; }
        public string Description { get; set; }
    }
}
