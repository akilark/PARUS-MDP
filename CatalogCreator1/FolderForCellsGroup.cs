using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WorkWithCatalog
{
	/// <summary>
	/// Класс необходимый для установления соответствия 
	/// группы ячеек с расположением необходимых файлов
	/// </summary>
	public class FolderForCellsGroup
	{
		private string _path;

		/// <summary>
		/// Конструктор с 3 параметрами
		/// </summary>
		/// <param name="path">путь к сформированному дереву папок </param>
		/// <param name="direction">направление перетока</param>
		/// <param name="schemeWithNumber">схема с её номером</param>
		public FolderForCellsGroup(string path, string direction, string schemeWithNumber)
		{
			string[] pathPart = new string[] { direction, schemeWithNumber };
			_path = path;
			for(int i = 0; i < pathPart.Length; i++)
			{
				_path = PathCheck(pathPart[i]);
			}
			
		}

		/// <summary>
		/// Поиск нужной директории для комбинации факторов
		/// </summary>
		/// <param name="factorsCombination">Комбинация факторов</param>
		/// <returns></returns>
		public string[] Find(string factorsCombination)
		{
			string factorPath = PathCheck(factorsCombination);
			return Directory.GetFiles(factorPath, @"*.xlsx");
		}

		private string PathCheck(string compareString)
		{
			var directorysArray = Directory.GetDirectories(_path);

			for (int i = 0; i < directorysArray.Length; i++)
			{
				if(FolderName(directorysArray[i]) == compareString)
				{
					return directorysArray[i];
				}
			}
			throw new ArgumentException($"В директории {_path} нет папки с названием {compareString}");
		}

		private string FolderName(string path)
		{
			while (path.Contains(@"\"))
			{
				path = path.Substring(path.IndexOf(@"\") + 1);
			}
			return path;
		}
	}
}
