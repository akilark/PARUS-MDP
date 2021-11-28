using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure.DataTypes
{
	public class AOCN
	{
		public string NodeName { get; set; }
		public string AutomaticName { get; set; }

		public ControlActionRow Automatic { get; set; }
	}
}
