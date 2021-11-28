using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure.DataTypes
{
	public class Imbalance
	{
		public string LineName { get; set; }
		public ControlActionRow ImbalanceValue { get; set; }

		public ControlActionRow ARPM { get; set; }
	}
}
