using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public class ImbalanceAndAutomatics
	{
		public string ImbalanceID { get; set; }
		public int ImbalanceValue { get; set; }
		public string ImbalanceCriterion { get; set; }
		public string Equation { get; set; }
		public float EquationValue { get; set; }
		public float ImbalanceCoefficient { get; set; }
		public int MaximumImbalance { get; set; }

	}
}
