
namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о небалансах активной мощности
	/// </summary>
	public class ImbalanceAndAutomatics
	{
		/// <summary>
		/// Идентификатор параметра УВ/НБ.
		/// </summary>
		public string ImbalanceID { get; set; }

		/// <summary>
		/// Значение небаланса (НБ)
		/// </summary>
		public int ImbalanceValue { get; set; }

		/// <summary>
		/// Критерий небаланса (НБ)
		/// </summary>
		public string ImbalanceCriterion { get; set; }

		/// <summary>
		/// Формула "НБ - К_Нбn*PНбn + ..."
		/// </summary>
		public string Equation { get; set; }

		/// <summary>
		/// Значение формулы "НБ - К_Нбn*PНбn + ..."
		/// </summary>
		public float EquationValue { get; set; }

		/// <summary>
		/// Значение коэффициента эффективности (К_Нбn)
		/// </summary>
		public float ImbalanceCoefficient { get; set; }

		/// <summary>
		/// Максимальное значение УВ/НБ (PНбn)
		/// </summary>
		public int MaximumImbalance { get; set; }

	}
}
