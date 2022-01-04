using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о допустимых перетоках активной мощности
	/// </summary>
	public class AllowPowerOverflows
	{
		/// <summary>
		/// Значение критерия 20%P
		/// </summary>
		public int StaticStabilityNormal { get; set; }

		/// <summary>
		/// Значение критерия 8%P
		/// </summary>
		public int StaticStabilityPostEmergency { get; set; }

		/// <summary>
		/// Значение критерия 10%U
		/// </summary>
		public int StabilityVoltageValue { get; set; }

		/// <summary>
		/// Определеющий ПАР для критерия 10%U
		/// </summary>
		public string StabilityVoltageCriterion { get; set; }

		/// <summary>
		/// Значение критерия АДТН
		/// </summary>
		public int CurrentLoadLinesValue { get; set; }

		/// <summary>
		/// Текст в столбце "Перегружаемый элемент" в файле ПК ПАРУС, для строки с минимальным АДТН
		/// </summary>
		public string CurrentLoadLinesCriterion { get; set; }

		/// <summary>
		/// Предельное значение перетока 
		/// </summary>
		public int CriticalValue { get; set; }

		/// <summary>
		/// Значение АДП
		/// </summary>
		public int EmergencyAllowPowerOverflow {get;set;}

		/// <summary>
		/// Текст в столбце "Примечание" в файле ПК ПАРУС
		/// </summary>
		public string Note { get; set; }

		/// <summary>
		/// Название линии, которая была отключена
		/// </summary>
		public string DisconnectionLineFact { get; set; }

	}
}
