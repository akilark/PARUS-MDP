
namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о небалансах. 
	/// Информация хранится в БД или заполняется пользователем вручную на этапе формирования дерева каталогов.
	/// </summary>
	public class ImbalanceDataSource
	{
		/// <summary>
		/// Название линии, небаланс при отключении которой рассматривают.
		/// Сравнивается с линией, отключаемой после возмущения.
		/// </summary>
		public string LineName { get; set; }

		/// <summary>
		/// Название небаланса.
		/// Сравнивается со столбцом "Признак параметра (УВ/НБ)".
		/// </summary>
		public string ImbalanceName { get; set; }

		/// <summary>
		/// Название автоматики разгрузки при перегрузки по мощности для рассматриваемого НБ.
		/// </summary>
		public string ARPMName { get; set; }

		/// <summary>
		/// Информация о значении небаланса.
		/// </summary>
		public ControlActionRow Imbalance { get; set; }

	}
}
