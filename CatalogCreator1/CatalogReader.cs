using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace WorkWithCatalog
{
	/// <summary>
	/// Класс необходимый для формирования данных о факторах, 
	/// реверсивности и схемах для выбранной директории
	/// </summary>
	public class CatalogReader
	{
		private string _rootName;
		private string[] _allScheme;
		private List<(string, (string, string[])[])> _factors = new List<(string, (string, string[])[])>();
		private List<string> _temperature = new List<string>();

		/// <summary>
		/// Свойство хранящее имя сечения
		/// </summary>
		public string SectionName => _rootName;

		/// <summary>
		/// Свойство хранящее массив всех рассматриваемых схем
		/// </summary>
		public string[] AllScheme => _allScheme;

		/// <summary>
		/// Свойство хранящее список влияющих факторов без указания направления мощности
		/// </summary>
		public List<(string, (string, string[])[])> Factors => _factors;

		/// <summary>
		/// Свойство хранящее сведения о температурах
		/// </summary>
		public List<string> Temperature => _temperature;

		/// <summary>
		/// Конструктор класса с 1 параметром
		/// </summary>
		/// <param name="path">Путь к содержащий в себе корневую папку</param>
		public CatalogReader(string path)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			FindRootName(path);
			FindFactorsEachDirections(path);
		}

		/// <summary>
		/// Определение Имени сечения
		/// </summary>
		/// <param name="path">путь в папку сечение</param>
		private void FindRootName(string path)
		{
			_rootName = FolderName(path);
		}


		/// <summary>
		/// проверка реверсивный ли переток
		/// </summary>
		/// <param name="path"></param>
		private void FindFactorsEachDirections(string path)
		{
			var directorysArray = Directory.GetDirectories(path);

			for (int i = 0; i < directorysArray.Length; i++)
			{
				_factors.Add((FolderName(directorysArray[i]), FindFactorsOneDirection(FindSchemeName(directorysArray[i]))));
			}
		}

		/// <summary>
		/// Определение имени схемы
		/// </summary>
		/// <param name="path">путь к папке для определенной схемы</param>
		/// <returns>имя схемы</returns>
		private string FindSchemeName(string path)
		{
			var directorysArray = Directory.GetDirectories(path);
			_allScheme = new string[directorysArray.Length];
			for (int index = 0; index < directorysArray.Length; index++)
			{
				_allScheme[index] = FolderName(directorysArray[index]);
			}
			return directorysArray[0];
		}

		/// <summary>
		/// определение факторов
		/// </summary>
		/// <param name="schemePath">путь к папке для определенного сочетания факторов</param>
		/// <returns> Список факторов</returns>
		private (string, string[])[] FindFactorsOneDirection(string schemePath)
		{
			var directoriesArray = Directory.GetDirectories(schemePath);
			if (directoriesArray.Length != 0)
			{
				FindTemperature(directoriesArray[0]);
				var factorsTmp = new (string, string[])[0];
				for (int i = 0; i < directoriesArray.Length; i++)
				{
					directoriesArray[i] = FolderName(directoriesArray[i]);
				}
				var factorsArray = new List<string[]>();
				string[] factorsAmount = directoriesArray[0].Split("_[");
				foreach (string directoryString in directoriesArray)
				{
					factorsArray.Add(directoryString.Split("_["));
				}
				for (int factorsIndex = 0; factorsIndex < factorsAmount.Length; factorsIndex++)
				{
					var factorValue = new string[0];
					var indexName = factorsArray[0][factorsIndex].IndexOf("]") + 1;
					var factorName = factorsArray[0][factorsIndex].Substring(indexName);
					for (int factorsValueIndex = 0; factorsValueIndex <
						factorsArray.Count; factorsValueIndex++)
					{
						var uniqueValue = true;
						var indexValue = factorsArray[factorsValueIndex][factorsIndex].IndexOf("]");
						var factorValueTmp =
							factorsArray[factorsValueIndex][factorsIndex].Substring(0, indexValue).Trim('[', ']');
						for (int z = 0; z < factorValue.Length; z++)
						{
							if (factorValueTmp == factorValue[z])
							{
								uniqueValue = false;
								break;
							}
						}
						if (uniqueValue)
						{
							Array.Resize(ref factorValue, factorValue.Length + 1);
							factorValue[factorValue.Length - 1] = factorValueTmp;
						}
					}
					Array.Resize(ref factorsTmp, factorsTmp.Length + 1);
					factorsTmp[factorsTmp.Length - 1] = (factorName, factorValue);
				}
				return factorsTmp;
			}
			else
			{
				FindTemperature(schemePath);
				return null;
			}

		}

		//TODO: добавить проверку данных на листе
		//TODO: Перенести в работу с экселем
		/// <summary>
		/// Определения направления
		/// </summary>
		/// <param name="factorsPath">путь к нижней папке в каталоге</param>
		private void FindTemperature(string factorsPath)
		{
			var directorysArray = Directory.GetFiles(factorsPath, @"*.xlsx");
			if (directorysArray.Length != 0)
			{
				FileInfo fileInfo = new FileInfo(directorysArray[0]);
				ExcelPackage excelPackage = new ExcelPackage(fileInfo);
				for (int i = 0; i < excelPackage.Workbook.Worksheets.Count; i++)
				{
					_temperature.Add(excelPackage.Workbook.Worksheets[i].Name);
				}
			}
		}

		/// <summary>
		/// Получение названия папки из ссылки на папку
		/// </summary>
		/// <param name="path">ссылка на папку</param>
		/// <returns>название папки</returns>
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

