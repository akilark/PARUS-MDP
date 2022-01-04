using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации об автоматике ограничения перегрузки оборудования
	/// </summary>
	public class AOPO
	{
		/// <summary>
		/// Название линии, при перегрузке которой срабатывает данная автоматика
		/// </summary>
		public string LineName { get; set; }

		/// <summary>
		/// Название автоматики.
		/// Сравнивается с "Признак параметра (УВ/НБ)" в файле шаблона.
		/// </summary>
		public string AutomaticName { get; set; }

		/// <summary>
		/// Информация об автоматике.
		/// </summary>
		public ControlActionRow Automatic {get;set;}
	}
}
