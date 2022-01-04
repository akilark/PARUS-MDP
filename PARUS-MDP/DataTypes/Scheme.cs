using System.Collections.Generic;


namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о схеме сети
	/// </summary>
	public class Scheme
	{
		/// <summary>
		/// Название схемы
		/// </summary>
		public string SchemeName { get; set; }

		/// <summary>
		/// Рассматриваемые возмущения для данной схемы
		/// item1 - название возмущения
		/// item2 - наличие ПА
		/// </summary>
		public List<(string,bool)> Disturbance { get; set; }
	}
}
