using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure.DataTypes
{
	public class AOPO
	{
		public string LineName { get; set; }

		public string AutomaticName { get; set; }

		public ControlActionRow Automatic {get;set;}
	}
}
