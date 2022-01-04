using System;
using System.Collections.Generic;
using System.Text;

namespace DataTypes
{
	/// <summary>
	/// Класс необходимый для хранения информации о группе ячеек в заполненном 
	/// файле шаблона, которые состовляют уникальную комбинацию направления перетока,
	/// схемы, фактора, значения фактора и температуры. 
	/// А также информация о расположении файлов ПК ПАРУС, в которых может содержаться 
	/// информация для данной группы ячеек.
	/// </summary>
	public class CellsGroup
	{
		/// <summary>
		/// Путь к папке, в которой находятся необходимые файлы ПК ПАРУС
		/// </summary>
		public string[] Folders { get; set; }

		/// <summary>
		/// Направление перетока
		/// </summary>
		public string Direction { get; set; }
		
		/// <summary>
		/// Название схемы
		/// </summary>
		public string SchemeName { get; set; }

		/// <summary>
		/// Рассматриваемые возмущения для данной схемы
		/// item1 - название возмущения
		/// item2 - наличие ПА
		/// </summary>
		public List<(string, bool)> Disturbance { get; set; }

		/// <summary>
		/// Название фактора и значение фактора.
		/// item1- Название.
		/// item2- Значение.
		/// </summary>
		public List<(string, int)> Factors { get; set; }
		
		/// <summary>
		/// Количество строк выделенное для группы ячеек
		/// </summary>
		public int SizeCellsArea { get; set; }
		
		/// <summary>
		/// item1 - первая свободная строка для группы ячеек 
		/// item2 - первый столбец после перечисления факторов (МДП без ПА)
		/// </summary>
		public (int, int) StartID { get; set; }

		/// <summary>
		/// Температура
		/// </summary>
		public int Temperature { get; set; }

		/// <summary>
		/// Зависимость от температуры
		/// </summary>
		public bool TemperatureDependence { get; set; }
	}
}
