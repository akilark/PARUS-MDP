using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure.DataTypes
{
	public class ImbalanceDataSource
	{
		public string LineName { get; set; }
		public string ImbalanceName { get; set; }
		public string ARPMName { get; set; }
		public ImbalanceAndAutomatics Imbalance { get; set; }
		public ImbalanceAndAutomatics ARPM { get; set; }

		public ControlAction ControlAction { get; set; }
	}
}
