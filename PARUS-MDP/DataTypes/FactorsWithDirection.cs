using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
	public class FactorsWithDirection
	{
		public List<(string, string[])> FactorNameAndValues { get; set; }

		public string Direction { get; set; }
	}
}
