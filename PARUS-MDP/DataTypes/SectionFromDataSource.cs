using System;
using System.Collections.Generic;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о сечении из БД
	/// </summary>
	[Serializable]
	public class SectionFromDataSource
	{
		public SectionFromDataSource() { }

		/// <summary>
		/// Название сечения
		/// </summary>
		public string SectionName { get; set; }

		/// <summary>
		/// Факторы характерные для данного сечения
		/// </summary>
		public List<FactorsWithDirection> Factors { get; set; }

		/// <summary>
		/// Схемы характерные для данного сечения
		/// </summary>
		public List<Scheme> Schemes { get; set; }

		/// <summary>
		/// Небалансы, которые рассматриваются для данного сечения
		/// </summary>
		public List<ImbalanceDataSource> Imbalances { get; set; }

		/// <summary>
		/// Список АОПО
		/// </summary>
		public List<AOPO> AOPOlist { get; set; }

		/// <summary>
		/// Список АОСН
		/// </summary>
		public List<AOCN> AOCNlist { get; set; }
	}
}
