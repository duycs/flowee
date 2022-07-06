using System;
using AppShareDomain.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class SettingType : Enumeration
    {
        public static SettingType Object = new SettingType(0, nameof(Object).ToLowerInvariant());
        public static SettingType Text = new SettingType(1, nameof(Text).ToLowerInvariant());
        public static SettingType Checkbox = new SettingType(2, nameof(Checkbox).ToLowerInvariant());
        public static SettingType List = new SettingType(3, nameof(List).ToLowerInvariant());

        public SettingType(int id, string name) : base(id, name)
        {
        }
    }
}

