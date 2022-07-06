using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
	public class Specification : Entity
	{
        public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }

		public virtual ICollection<Setting>? Settings { get; set; }
		public virtual ICollection<SpecificationSetting>? SpecificationSettings { get; set; }
	}
}

