﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации об автоматике ограничения снижения напряжения
	/// </summary>
	public class AOCN
	{
		/// <summary>
		/// Название узла, при понижении напряжения в котором, срабатывает автоматика
		/// </summary>
		public string NodeName { get; set; }

		/// <summary>
		/// Название автоматики.
		/// Сравнивается с "Признак параметра (УВ/НБ)" в файле шаблона.
		/// </summary>
		public string AutomaticName { get; set; }

		/// <summary>
		/// Информация об автоматике.
		/// </summary>
		public ControlActionRow Automatic { get; set; }
	}
}
