using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public class ControlAction
	{
		public string ParamID { get; set; }

		public float CoefficientEfficiency { get; set; }

		public int ActivePowerControlActionMax { get; set; }
		
		public (int,int) IDCell { get; set; }

		public string ParamSign { get; set; }
	}
}
