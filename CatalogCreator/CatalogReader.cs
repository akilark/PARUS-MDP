using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace CatalogCreator
{
	/// <summary>
	/// Класс необходимый для формирования данных о факторах, 
	/// реверсивности и схемах для выбранной директории
	/// </summary>
	public class CatalogReader
	{
		
		private string _path;
		private string _rootName;
		private string[] _allScheme;
		private List<(string, string[])> _factorsEast;
		private List<(string, string[])> _factorsWest;
		private List<(string, string[])> _factors;
		private List<(string, string[])> _temperature = new List<(string,string[])>();
		private string[] _directions;
		public bool _reverseable;

		/// <summary>
		/// Свойство хранящее массив всех рассматриваемых схем
		/// </summary>
		public string[] AllScheme
		{
			get
			{
				return _allScheme;
			}
		}

		/// <summary>
		/// Свойство хранящее список влияющих факторов на восток
		/// </summary>
		public List<(string, string[])> FactorsEast
		{
			get
			{
				return _factorsEast;
			}
		}

		/// <summary>
		/// Свойство хранящее список влияющих факторов на запад
		/// </summary>
		public List<(string, string[])> FactorsWest
		{
			get
			{
				return _factorsWest;
			}
		}

		/// <summary>
		/// Свойство хранящее список влияющих факторов без указания направления мощности
		/// </summary>
		public List<(string, string[])> Factors
		{
			get
			{
				return _factors;
			}
		}

		/// <summary>
		/// Свойство хранящее сведения о реверсивности для сечения
		/// </summary>
		public bool Reversable
		{
			get
			{
				return _reverseable;
			}
		}

		/// <summary>
		/// Свойство хранящее сведения о температурах
		/// </summary>
		public List<(string, string[])> Temperature
		{
			get
			{
				return _temperature;
			}
		}

		/// <summary>
		/// Конструктор класса с 1 параметром
		/// </summary>
		/// <param name="path">Путь к содержащий в себе корневую папку</param>
		public CatalogReader(string path)
		{
			_path = path;
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			FindRootName(path);
			ReversableCheck(path);
		}

		private void FindRootName(string path)
		{
			_rootName = FolderName(path);
		}

		private void ReversableCheck(string path)
		{
			var directorysArray = Directory.GetDirectories(path);
			if (directorysArray.Length == 2)
			{
				if ((directorysArray[0].Contains(_directions[0]) && 
					directorysArray[1].Contains(_directions[1])))
				{
					_reverseable = true;
					_factorsWest = FindFactors(FindSchemeName(directorysArray[0]));
					_factorsEast = FindFactors(FindSchemeName(directorysArray[1]));
				}
			}
			else
			{
				_factors = FindFactors(FindSchemeName(path));
			}
		}

		private string FindSchemeName(string path)
		{
			var directorysArray = Directory.GetDirectories(path);
			_allScheme = new string[directorysArray.Length];
			for(int index = 0; index < directorysArray.Length; index++)
			{
				_allScheme[index] = FolderName(directorysArray[index]);
			}
			return directorysArray[0];
		}

		private List<(string, string[])> FindFactors(string schemePath)
		{
			var directoriesArray = Directory.GetDirectories(schemePath);
			if (directoriesArray.Length != 0)
			{
				FindTemperature(directoriesArray[0]);
				var factorsTmp = new List<(string, string[])>();
				for (int i = 0; i < directoriesArray.Length; i++)
				{
					directoriesArray[i] = FolderName(directoriesArray[i]);
				}
				var factorsArray = new List<string[]>();
				string[] factorsAmount = directoriesArray[0].Split("_");
				foreach (string directoryString in directoriesArray)
				{
					factorsArray.Add(directoryString.Split("_"));
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
					factorsTmp.Add((factorName, factorValue));
				}
				return factorsTmp;
			}
			else
			{
				FindTemperature(schemePath);
				return null;
			}
			
		}

		/// <summary>
		/// Считает, что для всех схем одни и теже температуры, 
		/// но температуры могут быть разные в зависимости направления мощности 
		/// </summary>
		/// <param name="factorsPath"></param>
		private void FindTemperature(string factorsPath)
		{
			string direction = "без направления";
			if(_reverseable)
			{
				foreach(string dir in _directions)
				{
					if (factorsPath.Contains(dir))
					{
						direction = dir;
					}
				}
				
			}
			var directorysArray = Directory.GetFiles(factorsPath);
			string[] temperature;
			if (directorysArray.Length != 0)
			{

				FileInfo fileInfo = new FileInfo(directorysArray[0]);
				ExcelPackage excelPackage = new ExcelPackage(fileInfo);
				temperature = new string[excelPackage.Workbook.Worksheets.Count];
				for(int i = 0; i < excelPackage.Workbook.Worksheets.Count; i++)
				{
					temperature[i] = excelPackage.Workbook.Worksheets[i].Name;
				}
				_temperature.Add((direction,temperature));
			}
			else
			{
				temperature = new string[0];
				_temperature.Add((direction, temperature));
			}
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
