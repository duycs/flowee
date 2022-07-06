using System;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
	public class SpecificationSetting : Entity
	{
		public int SettingId { get; set; }
		public Setting Setting { get; set; }
		public int SpecificationId { get; set; }
		public Specification Specification { get; set; }
	}
}

