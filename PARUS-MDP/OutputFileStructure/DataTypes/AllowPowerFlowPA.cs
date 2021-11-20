using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public class AllowPowerFlowPA
	{
		public int ValueWithPA { get; set; }
		public int LocalAutomaticValueWitoutPA { get; set; }

		public string CriteriumValueWithPA { get; set; }
		public string CriteriumLocalAutomaticValueWithoutPA { get; set; }

		public int EqupmentOverloadingWithoutPA { get; set; }

		public string CriteriumEqupmentOverloadingWithoutPA { get; set; }

		public string DisconnectionLineFactEqupmentOverloading { get; set; }

		public string CriteriumEqupmentOverloadingWithtPA { get; set; }

		public int VoltageLimitingWithoutPA { get; set; }


		public string DisconnectionLineFactVoltageLimiting { get; set; }

		public string CriteriumVoltageLimitingWithtPA { get; set; }
	}
}
