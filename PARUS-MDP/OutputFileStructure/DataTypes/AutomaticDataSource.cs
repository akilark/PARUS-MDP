using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure.DataTypes
{
	public class AutomaticDataSource
	{
		public string LineName { get; set; }
		public List<string> AutomaticsName { get; set; }

		public List<ImbalanceAndAutomatics> Automatics {get;set;}
	}
}
