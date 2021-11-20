using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public class AllowPowerOverflows
	{
		public int StaticStabilityNormal { get; set; }
		public int StaticStabilityPostEmergency { get; set; }
		public int StabilityVoltageValue { get; set; }
		public string StabilityVoltageCriterion { get; set; }
		public int CurrentLoadLinesValue { get; set; }
		public string CurrentLoadLinesCriterion { get; set; }
		public int CriticalValue { get; set; }
		public int EmergencyAllowPowerOverflow {get;set;}
		public string Note { get; set; }
		public string DisconnectionLineFact { get; set; }

	}
}
