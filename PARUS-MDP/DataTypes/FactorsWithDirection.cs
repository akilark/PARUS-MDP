using System.Collections.Generic;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о факторах для 
	/// конкретного направления перетока мощности
	/// </summary>
	public class FactorsWithDirection
	{
		/// <summary>
		/// Название фактора и значения фактора.
		/// item1- Название.
		/// item2- Значения.
		/// </summary>
		public List<(string, string[])> FactorNameAndValues { get; set; }

		/// <summary>
		/// Направление перетока активной мощности.
		/// </summary>
		public string Direction { get; set; }
	}
}
