using System;
using System.Collections.Generic;
using System.IO;

namespace CatalogCreator
{
	/// <summary>
	/// Класс для создания дерева каталогов
	/// </summary>
	public class CatalogCreator
	{
		private string _path;
		private string _rootName;
		private string[] _repairScheme;
		private string[] _allScheme;
		private List<(string, string[])> _factorsEast;
		private List<(string, string[])> _factorsWest;
		private List<(string, string[])> _factors;
		private string[] _directions;
		private const string _directionEastTitle = "На восток";
		private const string _directionsWestTitle = "На запад";
		public bool _reverseable;
		public bool _flagDoubleRepair = true;
		public bool _flagRepair; //Если только нормальная схема - false
		
		private string _sectionName;
		private List<string> _sections = new List<string>();
		private List<(string, (string, string[])[])> _factorsы = new List<(string, (string, string[])[])>();
		private List<(string, (string, bool)[])> _schemes = new List<(string, (string, bool)[])>();



		public string[] Directions
		{
			get
			{
				return _directions;
			}
			set
			{
				_directions = value;
			}
		}
		/// <summary>
		/// Конструктор класса с 2 параметрами
		/// </summary>
		/// <param name="path"></param>
		/// <param name="rootName"></param>
		public CatalogCreator(string path, string rootName)
		{
			_reverseable = false;
			_flagRepair = false; 
		}

		/// <summary>
		/// Конструктор класса без параметров
		/// </summary>
		public CatalogCreator() { }

		/// <summary>
		/// Свойство хранящее путь где будет создан каталог 
		/// </summary>
		public string DirectoryPath
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}

		/// <summary>
		/// Свойство хранящее имя корневой папки (название сечения)
		/// </summary>
		public string RootName 
		{
			get
			{
				return _rootName;
			}
			set
			{
				_rootName = value;
			}
		}

		/// <summary>
		/// Свойство хранящее массив ремонтных схем
		/// </summary>
		public string[] RepairScheme
		{
			get
			{
				return _repairScheme;
			}
			set
			{
				_repairScheme = value;
				_flagRepair = true;
			}
		}

		/// <summary>
		/// Свойство хранящее массив всех рассматриваемых схем
		/// </summary>
		public string[] AllScheme
		{
			get
			{
				return _allScheme;
			}
			set
			{
				_allScheme = value;
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
			set
			{
				_factorsEast = value;
				if (FactorsWest != null)
				{
					_reverseable = true;
				}
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
			set
			{
				_factorsWest = value;

				if (FactorsEast != null)
				{
					_reverseable = true;
				}
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
			set
			{
				_factors = value;
				_reverseable = false;
			}
		}

		/// <summary>
		/// Функция формирующая каталог на основе входных данных
		/// </summary>
		public void Create()
		{
			var pathRoot = Path.Combine(_path, _rootName);
			Directory.CreateDirectory(pathRoot);
			CreateReversable(pathRoot);
		}

		/// <summary>
		/// Создание папки с названием направленя мощности
		/// </summary>
		/// <param name="pathRoot">путь к корневой папке</param>
		private void CreateReversable(string pathRoot)
		{
			if(_reverseable == true)
			{
				foreach (string direct in Directions)
				{
					var pathReversable = Path.Combine(pathRoot, direct);
					Directory.CreateDirectory(pathReversable);
					CreateScheme(pathReversable);
				}
			}
			else
			{
				CreateScheme(pathRoot);
			}
		}

		/// <summary>
		/// Преобразующий входные данные о ремонтных схем в правильный вид 
		/// в зависимости от условий
		/// </summary>
		private void ExpandSchemeArray()
		{
			if (_flagRepair)
			{
				if (_flagDoubleRepair)
				{
					var doubleRepair = new DoubleRepair(RepairScheme);
					var repairTmp = new string[RepairScheme.Length + 
						doubleRepair.DoubleRepairSchemeName.Length + 1];
					repairTmp[0] = "Нормальная схема";
					RepairScheme.CopyTo(repairTmp, 1);
					doubleRepair.DoubleRepairSchemeName.CopyTo(repairTmp, 
						RepairScheme.Length + 1);
					_allScheme = repairTmp;
				}
				else
				{
					var repairTmp = new string[RepairScheme.Length + 1];
					repairTmp[0] = "Нормальная схема";
					RepairScheme.CopyTo(repairTmp, 1);
					_allScheme = repairTmp;
				}
			}
			else
			{
				var repairTmp = new string[1];
				repairTmp[0] = "Нормальная схема";
				_allScheme = repairTmp;
			}
		}

		/// <summary>
		/// Метод создающий папки с названием схем
		/// </summary>
		/// <param name="pathReversable">путь к папке с названием 
		/// направления мощности</param>
		private void CreateScheme(string pathReversable)
		{
			var serialNumber = 1;
			ExpandSchemeArray();
			foreach (string scheme in _allScheme)
			{
				string pathScheme = Path.Combine(pathReversable, "№"+ serialNumber+ "_" + scheme);
				Directory.CreateDirectory(pathScheme);
				serialNumber++;

				if (_reverseable)
				{
					DirectionFactors(pathScheme);
				}
				else
				{
					CreateFactorsCatalog(pathScheme, _factors);
				}
			}
		}

		/// <summary>
		/// Метод определяющий директорию для направления перетока
		/// </summary>
		/// <param name="pathScheme">Путь к папке в которой 
		/// производится определение</param>
		private void DirectionFactors(string pathScheme)
		{
			
			if (pathScheme.Contains(_directionEastTitle))
			{
				CreateFactorsCatalog(pathScheme, _factorsEast);
			}
			else if (pathScheme.Contains(_directionsWestTitle))
			{
				CreateFactorsCatalog(pathScheme, _factorsWest);
			}
			else
			{
				CreateFactorsCatalog(pathScheme, _factors);
			}
		}

		/// <summary>
		/// Метод отвечающий за создание подпапок с названием влияющих факторов
		/// </summary>
		/// <param name="pathDirection">путь к папке в которой происходит создание
		/// </param>
		/// <param name="factors">Лист кортежей содержащий в 
		/// item1 -название влияющиего фактора, в item2- значения фактора</param>
		/// <returns> массив уникальных наборов факторов</param>
		private void CreateFactorsCatalog(string pathDirection, 
			List<(string, string[])> factors)
		{
			if (factors!=null)
			{
				switch (factors.Count)
				{
					case 0:
					{
						break;
					}
					case 1:
					{
						CreateOneFactor(pathDirection, factors);
						break;
					}
					default:
					{
						CreateFewFactors(pathDirection, factors);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Метод создающий папки с названием уникальных наборов факторов
		/// </summary>
		/// <param name="pathDirection"> путь к папке в которой происходит создание
		/// </param>
		/// <param name="factors">Лист кортежей содержащий в 
		/// item1 -название влияющиего фактора, в item2- значения фактора</param>
		/// <returns> массив уникальных наборов факторов</param>
		private void CreateFewFactors(string pathDirection, 
			List<(string, string[])> factors)
		{
			foreach(string stringMiexedFactors in GenerateMixedFactors(factors))
			{
				var pathFactor = Path.Combine(pathDirection, stringMiexedFactors);
				Directory.CreateDirectory(pathFactor);
			}
		}

		/// <summary>
		/// Метод создающий массив уникальных наборов факторов
		/// </summary>
		/// <param name="factors">Лист кортежей содержащий в 
		/// item1 -название влияющиего фактора, в item2- значения фактора</param>
		/// <returns> массив уникальных наборов факторов</returns>
		private string[] GenerateMixedFactors(List<(string, string[])> factors)
		{
			var amountFactorValues = AmountFactorsValueCalculate(factors);
			var factorsMixedSize = 1;

			for(int i= 0; i < amountFactorValues.Length; i++)
			{
				factorsMixedSize +=  amountFactorValues[i];
			}

			var factorsMixed = new string[factorsMixedSize];
			var delimer = 1;
			var areaSize = factorsMixedSize;
			for (int currentFactor = 0; currentFactor < factors.Count; currentFactor++)
			{
				if(currentFactor == 0)
				{
					delimer *= amountFactorValues[currentFactor];
				}
				else
				{
					delimer *= amountFactorValues[currentFactor] / amountFactorValues[currentFactor - 1];
				}
				areaSize /= delimer;

				var counterUnitInArea = 0;
				var factorIndex = 0;
				for (int areaIndex = 0; areaIndex < factorsMixedSize; areaIndex++)
				{
					if (counterUnitInArea == areaSize)
					{
						factorIndex++;
						if (factorIndex == amountFactorValues[currentFactor])
						{
							factorIndex = 0;
						}
						counterUnitInArea = 0;
					}

					if(currentFactor == 0)
					{
						factorsMixed[areaIndex] = "[" + factors[currentFactor].Item2[factorIndex] + "]" +
							factors[currentFactor].Item1;
					}
					else
					{
						factorsMixed[areaIndex] = factorsMixed[areaIndex] +
							"_[" + factors[currentFactor].Item2[factorIndex] + "]" +
							factors[currentFactor].Item1;
					}
					counterUnitInArea++;
				}
			}
			return factorsMixed;
		}

		/// <summary>
		/// Метод для расчета количества уникальных комбинаций значений 
		/// влияющих факторов
		/// </summary>
		/// <param name="factors">Лист кортежей содержащий в 
		/// item1 -название влияющиего фактора, в item2- значения фактора</param>
		/// <returns>Возвращает количество уникальных комбинаций 
		/// значений влияющих факторов</returns>
		private int[] AmountFactorsValueCalculate(List<(string, string[])> factors)
		{
			var amountFactorValues = new int[0];
			foreach ((string, string[]) factor in factors)
			{
				Array.Resize(ref amountFactorValues, amountFactorValues.Length + 1);
				amountFactorValues[amountFactorValues.Length - 1] = factor.Item2.Length;

			}
			return amountFactorValues;
		}

		/// <summary>
		/// Метод для создания папок влияющих факторов, 
		/// если только 1 влияющий фактор
		/// </summary>
		/// <param name="pathDirection">путь к папке, чьей подпапкой 
		/// будут являться создаваемые папки</param>
		/// <param name="factors">Лист кортежей содержащий в 
		/// item1 -название влияющиего фактора, в item2- значения фактора </param>
		private void CreateOneFactor(string pathDirection, 
			List<(string, string[])> factors)
		{
			foreach (string factorValue in factors[0].Item2)
			{
				var pathFactor = Path.Combine(pathDirection, 
					"[" + factorValue + "]" +" "+ factors[0].Item1);
				Directory.CreateDirectory(pathFactor);
			}
		}
	}
}
