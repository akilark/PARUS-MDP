using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о допустимых перетоках активной мощности с ПА
	/// </summary>
	public class AllowPowerFlowPA
	{
		/// <summary>
		/// Значение МДП с ПА
		/// </summary>
		public int ValueWithPA { get; set; }
		
		/// <summary>
		/// Значение МДП без ПА ЛАПНУ
		/// </summary>
		public int LocalAutomaticValueWithoutPA { get; set; }

		/// <summary>
		/// Критерий МДП с ПА
		/// </summary>
		public string CriteriumValueWithPA { get; set; }

		/// <summary>
		/// Критерий МДП без ПА ЛАПНУ
		/// </summary>
		public string CriteriumLocalAutomaticValueWithoutPA { get; set; }

		/// <summary>
		/// Значение МДП без ПА АОПО
		/// </summary>
		public int EqupmentOverloadingWithoutPA { get; set; }

		/// <summary>
		/// критерий МДП без ПА АОПО
		/// </summary>
		public string CriteriumEqupmentOverloadingWithoutPA { get; set; }

		/// <summary>
		/// Название линии, которая была отключена для определения МДП без ПА АОПО
		/// </summary>
		public string DisconnectionLineFactEqupmentOverloading { get; set; }

		/// <summary>
		/// Критерий МДП с ПА АОПО
		/// </summary>
		public string CriteriumEqupmentOverloadingWithPA { get; set; }

		/// <summary>
		/// Значение МДП без ПА АОСН
		/// </summary>
		public int VoltageLimitingWithoutPA { get; set; }

		/// <summary>
		/// Название линии, которая была отключена для определения МДП без ПА АОСН
		/// </summary>
		public string DisconnectionLineFactVoltageLimiting { get; set; }

		/// <summary>
		/// Критерий МДП с ПА АОСН
		/// </summary>
		public string CriteriumVoltageLimitingWithPA { get; set; }

		/// <summary>
		/// Список управляющих воздействий ЛАПНУ
		/// </summary>
		public List<ControlActionRow> ControlActionsLAPNY { get; set; }

		/// <summary>
		/// Список управляющих воздействий АОПО
		/// </summary>
		public ControlActionRow ControlActionAOPO { get; set; }

		/// <summary>
		/// Список управляющих воздействий АОСН
		/// </summary>
		public ControlActionRow ControlActionAOCN { get; set; }
	}
}
