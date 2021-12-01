using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	public class AllowPowerFlowPA
	{
		public int ValueWithPA { get; set; }
		public int LocalAutomaticValueWithoutPA { get; set; }

		public string CriteriumValueWithPA { get; set; }
		public string CriteriumLocalAutomaticValueWithoutPA { get; set; }

		public int EqupmentOverloadingWithoutPA { get; set; }

		public string CriteriumEqupmentOverloadingWithoutPA { get; set; }

		public string DisconnectionLineFactEqupmentOverloading { get; set; }

		public string CriteriumEqupmentOverloadingWithPA { get; set; }

		public int VoltageLimitingWithoutPA { get; set; }


		public string DisconnectionLineFactVoltageLimiting { get; set; }

		public string CriteriumVoltageLimitingWithPA { get; set; }

		public List<ControlActionRow> ControlActionsLAPNY { get; set; }

		public ControlActionRow ControlActionAOPO { get; set; }

		public ControlActionRow ControlActionAOCN { get; set; }
	}
}
