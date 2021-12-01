using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	public class CellsGroup
	{
		
		public string[] Folders { get; set; }

		public string Direction { get; set; }
		
		public string SchemeName { get; set; }
		public bool AutomaticForScheme { get; set; }
		
		public List<(string, int)> Factors { get; set; }
		
		public int SizeCellsArea { get; set; }
		
		public (int, int) StartID { get; set; }

		public int Temperature { get; set; }
	}
}
