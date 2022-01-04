
namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о МДП, АДП и критериях их определения
	/// </summary>
	public class MaximumAllowPowerFlow
	{
		/// <summary>
		/// Значение МДП
		/// </summary>
		public int MaximumAllowPowerFlowValue { get; set; }

		/// <summary>
		/// Определяющий критерий для МДП
		/// </summary>
		public string MaximumAllowPowerCriterion { get; set; }

		/// <summary>
		/// Значение АДП
		/// </summary>
		public int EmergencyAllowPowerFlowValue { get; set; }

		/// <summary>
		/// Определяющий критерий для АДП
		/// </summary>
		public string EmergencyAllowPowerCriterion { get; set; }
	}
}
