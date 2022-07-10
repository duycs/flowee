using System;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
	public class RuleSetting : Entity
	{
		public int SettingId { get; set; }
		public Setting Setting { get; set; }
		public int RuleId { get; set; }
		public Rule Rule { get; set; }
	}
}

