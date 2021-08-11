using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogCreator
{
	/// <summary>
	/// Класс двойных ремонтов (В текущей реализации не используется)
	/// </summary>
	public class DoubleRepair
	{
		private string[] _repairSchemeName;
		private string[] _doubleRepairSchemeName = new string[0];

		/// <summary>
		/// Свойство хранящее массив данных двойных ремонтных схем
		/// </summary>
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

		/// <summary>
		/// Конструктор класса c 1 параметром
		/// </summary>
		/// <param name="repairSchemeName"></param>
		public DoubleRepair(string[] repairSchemeName)
		{
			_repairSchemeName = repairSchemeName;
			if (repairSchemeName.Length == 0)
			{
				throw new Exception("Для определения схем двойных ремонтов необходимо " +
					"задать схемы одинарных ремонтов");
			}
			GenerateDoubleRepairSchemeName();
		}

		/// <summary>
		/// Метод производящий подсчет двойных ремонтных схем
		/// </summary>
		/// <returns>количество двойных ремонтных схем</returns>
		private int DoubleSchemeCount()
		{
			int doubleSchemeSize = 0;
			for (int schemeSize = _repairSchemeName.Length - 1; schemeSize > 0; 
				schemeSize --)
			{
				doubleSchemeSize = doubleSchemeSize + schemeSize;
			}
			return doubleSchemeSize;
		}

		/// <summary>
		/// Создать все комбинации двойных ремонтных схем
		/// </summary>
		private void GenerateDoubleRepairSchemeName()
		{
			Array.Resize(ref _doubleRepairSchemeName, DoubleSchemeCount());
			int member = 0;
			for (int i = 0; i < _repairSchemeName.Length; i++)
			{
				for(int j = i + 1; j < _repairSchemeName.Length; j++)
				{
					_doubleRepairSchemeName[member] = (_repairSchemeName[i] + "; " +
						_repairSchemeName[j]);
					member = member + 1;
				}
			}
		}
	}
}
