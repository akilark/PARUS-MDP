using System;
using System.Collections.Generic;
using System.IO;
using DataTypes;

namespace WorkWithCatalog
{
	/// <summary>
	/// Класс для создания дерева каталогов
	/// </summary>
	public class CatalogCreator
	{
		private string _path;
		private List<FactorsWithDirection> _factors = new List<FactorsWithDirection>();
		private List<Scheme> _schemes = new List<Scheme>();
		private string _sectionName;

		/// <summary>
		/// Коструктор класса с 3 параметрами
		/// </summary>
		/// <param name="path"></param>
		/// <param name="rootName"></param>
		/// <param name="Factors">Лист факторов со структурой (направление, (фактор, значение фактора[])[])</param>
		/// <param name="Shemes">Лист схем со структурой (ремонтная схема, (возмущение, наличие противоаварийной автоматики)[])</param>
		public CatalogCreator(string path, string sectionName ,List<FactorsWithDirection> Factors, List<Scheme> Shemes)
		{
			_path = path;
			_factors = Factors;
			_schemes = Shemes;
			_sectionName = sectionName;
		}


		/// <summary>
		/// Функция формирующая каталог на основе входных данных
		/// </summary>
		public void Create()
		{
			DirectoryInfo dir = new DirectoryInfo(_path + @$"\{_sectionName}");
			dir.Create();
			dir.Attributes = FileAttributes.Normal;
			CreateReversable(_path + @$"\{_sectionName}");
		}

		/// <summary>
		/// Создание папки с названием направленя мощности
		/// </summary>
		/// <param name="pathRoot">путь к корневой папке</param>
		private void CreateReversable(string pathRoot)
		{
			for (int i = 0; i <  _factors.Count; i++)
			{
				var pathReversable = Path.Combine(pathRoot, _factors[i].Direction);
				DirectoryInfo dir = new DirectoryInfo(pathReversable);
				dir.Create();
				dir.Attributes = FileAttributes.Normal;
				
				CreateScheme(pathReversable);
			}
		}

		/// <summary>
		/// Преобразующий входные данные о ремонтных схем в правильный вид 
		/// в зависимости от условий
		/// </summary>
		private string[] SchemeArray()
		{
			var schemeArraySize = _schemes.Count;
			var allScheme = new string[schemeArraySize];
			for (int schemeIndex = 0; schemeIndex < schemeArraySize; schemeIndex++)
			{
				allScheme[schemeIndex] = _schemes[schemeIndex].SchemeName;
			}
			return allScheme;
		}

		/// <summary>
		/// Метод создающий папки с названием схем
		/// </summary>
		/// <param name="pathReversable">путь к папке с названием 
		/// направления мощности</param>
		private void CreateScheme(string pathReversable)
		{
			var serialNumber = 1;
			var allScheme = SchemeArray();
			foreach (string scheme in allScheme)
			{
				string pathScheme = Path.Combine(pathReversable, "№" + serialNumber + "_" + scheme);
				DirectoryInfo dir = new DirectoryInfo(pathScheme);
				dir.Create();
				dir.Attributes = FileAttributes.Normal;
				
				serialNumber++;

				DirectionFactors(pathScheme);
			}
		}

		/// <summary>
		/// Метод определяющий директорию для направления перетока
		/// </summary>
		/// <param name="pathScheme">Путь к папке в которой 
		/// производится определение</param>
		private void DirectionFactors(string pathScheme)
		{
			if (pathScheme.Contains(_factors[0].Direction))
			{
				CreateFactorsCatalog(pathScheme, _factors[0].FactorNameAndValues);
			}
			if (_factors.Count == 2)
			{
				if (pathScheme.Contains(_factors[1].Direction))
				{
					CreateFactorsCatalog(pathScheme, _factors[1].FactorNameAndValues);
				}
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
			if (factors != null)
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
			foreach (string stringMiexedFactors in GenerateMixedFactors(factors))
			{
				var pathFactor = Path.Combine(pathDirection, stringMiexedFactors);
				DirectoryInfo dir = new DirectoryInfo(pathFactor);
				dir.Create();
				dir.Attributes = FileAttributes.Normal;
				
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

			for (int i = 0; i < amountFactorValues.Length; i++)
			{
				factorsMixedSize += amountFactorValues[i];
			}

			var factorsMixed = new string[factorsMixedSize];
			var delimer = 1;
			var areaSize = factorsMixedSize;
			for (int currentFactor = 0; currentFactor < factors.Count; currentFactor++)
			{
				if (currentFactor == 0)
				{
					delimer *= amountFactorValues[currentFactor];
				}
				else
				{
					delimer = delimer * amountFactorValues[currentFactor] / amountFactorValues[currentFactor - 1];
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

					if (currentFactor == 0)
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
					"[" + factorValue + "]" + factors[0].Item1);
				DirectoryInfo dir = new DirectoryInfo(pathFactor);
				dir.Create();
				dir.Attributes = FileAttributes.Normal;
				
			}
		}
	}
}
