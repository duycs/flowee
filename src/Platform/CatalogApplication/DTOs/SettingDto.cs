using AppShareDomain.DTOs;
using AppShareServices.Models;

namespace CatalogApplication.DTOs
{
    public class SettingDto : DtoBase
    {
        public EnumerationDto SettingType { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
