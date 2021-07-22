using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogCreator
{
	class DoubleRepair
	{
		private string[] _repairSchemeName;
		private string[] _doubleRepairSchemeName = new string[0];

		private string[] RepairSchemeName
		{
			get
			{
				return _repairSchemeName;
			}
			set
			{
				_repairSchemeName = value;
			}
		}

		public string[] DoubleRepairSchemeName
		{
			get
			{				
				return _doubleRepairSchemeName;
			}
			private set
			{
				_doubleRepairSchemeName = value;
			}
		}

		public DoubleRepair(string[] repairSchemeName)
		{
			RepairSchemeName = repairSchemeName;
			if (repairSchemeName.Length == 0)
			{
				throw new Exception("Для расчета двойных ремонтов необходимо задать одинарные ремонты");
			}
			GenerateDoubleRepairSchemeName();
		}

		private int DoubleSchemeCount()
		{
			int doubleSchemeSize = 0;
			for (int schemeSize = RepairSchemeName.Length - 1; schemeSize > 0; schemeSize --)
			{
				doubleSchemeSize = doubleSchemeSize + schemeSize;
			}
			return doubleSchemeSize;
		}

		private void GenerateDoubleRepairSchemeName()
		{
			Array.Resize(ref _doubleRepairSchemeName, DoubleSchemeCount());
			int member = 0;
			for (int i = 0; i < RepairSchemeName.Length; i++)
			{
				for(int j = i + 1; j < RepairSchemeName.Length; j++)
				{
					_doubleRepairSchemeName[member] = (RepairSchemeName[i] + "; " + RepairSchemeName[j]);
					member = member + 1;
				}
			}
		}
	}
}
