
namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о небалансе.
	/// Экземпляр класса формируется после сравнения данных их БД и шаблона.
	/// </summary>
	public class Imbalance
	{
		/// <summary>
		/// Название линии, небаланс при отключении которой рассматривают.
		/// </summary>
		public string LineName { get; set; }

		/// <summary>
		/// Информация о значении небаланса.
		/// </summary>
		public ControlActionRow ImbalanceValue { get; set; }

		/// <summary>
		/// Информация о значении автоматики разгрузки при перегрузки по мощности для рассматриваемого НБ.
		/// </summary>
		public ControlActionRow ARPM { get; set; }
	}
}
