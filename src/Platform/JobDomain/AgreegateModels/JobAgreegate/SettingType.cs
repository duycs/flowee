using System;
using AppShareDomain.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class SettingType : Enumeration
    {
        public static SettingType Object = new SettingType(1, nameof(Object).ToLowerInvariant());
        public static SettingType Text = new SettingType(2, nameof(Text).ToLowerInvariant());
        public static SettingType Checkbox = new SettingType(3, nameof(Checkbox).ToLowerInvariant());
        public static SettingType List = new SettingType(4, nameof(List).ToLowerInvariant());
        public static SettingType Number = new SettingType(5, nameof(Number).ToLowerInvariant());

        public SettingType(int id, string name) : base(id, name)
        {
        }
    }
}

