using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
	public class Scheme
	{
		public string SchemeName { get; set; }
		public List<(string,bool)> Disturbance { get; set; }
	}
}
