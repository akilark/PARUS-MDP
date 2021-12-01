using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	public class ImbalanceDataSource
	{
		public string LineName { get; set; }
		public string ImbalanceName { get; set; }
		public string ARPMName { get; set; }

		public ControlActionRow Imbalance { get; set; }

	}
}
