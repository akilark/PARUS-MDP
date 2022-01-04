
namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о УВ/ НБ из файла шаблона
	/// </summary>
	public class ControlActionRow
	{
		/// <summary>
		/// Идентификатор параметра
		/// </summary>
		public string ParamID { get; set; }

		/// <summary>
		/// Признак параметра (УВ/НБ)
		/// </summary>
		public string ParamSign { get; set; }

		/// <summary>
		/// Направление перетока активной мозности для учета УВ/НБ
		/// </summary>
		public string Direction { get; set; }

		/// <summary>
		/// Максимальное значение УВ/НБ
		/// </summary>
		public int MaxValue { get; set; }

		/// <summary>
		/// Коэффициент эффективности
		/// </summary>
		public float CoefficientEfficiency { get; set; }

		
	}
}
