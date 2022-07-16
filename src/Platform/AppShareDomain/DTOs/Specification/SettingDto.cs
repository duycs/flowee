namespace AppShareDomain.DTOs.Catalog
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
