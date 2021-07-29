using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace CatalogCreator
{
	public class SampleReader
	{
		private string _path;
		private string _rootName;
		private string[] _allScheme;
		private List<(string, string[])> _factorsEast;
		private List<(string, string[])> _factorsWest;
		private List<(string, string[])> _factors;
		private List<(string, string[])> _temperature = new List<(string, string[])>();
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
		/// Конструктор класса с 1 параметром
		/// </summary>
		/// <param name="path">Путь к содержащий в себе корневую папку</param>
		public SampleReader(string path)
		{
			_path = path;
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		private void openExcel()
		{
			FileInfo fileInfo = new FileInfo(_path);
			ExcelPackage excelPackage = new ExcelPackage(fileInfo);
		}

		
	}
}
