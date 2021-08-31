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
		private List<(string, (string, string[])[])> _factors = new List<(string, (string, string[])[])>();
		private List<(string, (string, bool)[])> _schemes = new List<(string, (string, bool)[])>();

		/// <summary>
		/// Коструктор класса с 4 параметрами
		/// </summary>
		/// <param name="path"></param>
		/// <param name="rootName"></param>
		/// <param name="Factors">Лист факторов со структурой (направление, (фактор, значение фактора[])[])</param>
		/// <param name="Shemes">Лист схем со структурой (ремонтная схема, (возмущение, наличие противоаварийной автоматики)[])</param>
		public CatalogCreator(string path, string rootName, List<(string, (string, string[])[])> Factors, List<(string, (string, bool)[])> Shemes)
		{
			_path = path;
			_rootName = rootName;
			_factors = Factors;
			_schemes = Shemes;
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
			foreach ((string, (string, string[])[]) direct in _factors)
			{
				var pathReversable = Path.Combine(pathRoot, direct.Item1);
				Directory.CreateDirectory(pathReversable);
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
				allScheme[schemeIndex] = _schemes[schemeIndex].Item1;
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
				string pathScheme = Path.Combine(pathReversable, "№"+ serialNumber+ "_" + scheme);
				Directory.CreateDirectory(pathScheme);
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
			if (pathScheme.Contains(_factors[0].Item1))
			{
				List<(string, string[])> factorList = new List<(string, string[])>();
				foreach ((string, string[]) factor in _factors[0].Item2)
				{
					factorList.Add(factor);
				}
				CreateFactorsCatalog(pathScheme, factorList);
			}
			if (_factors.Count == 2)
			{
				if (pathScheme.Contains(_factors[1].Item1))
				{
					List<(string, string[])> factorList = new List<(string, string[])>();
					foreach ((string, string[]) factor in _factors[1].Item2)
					{
						factorList.Add(factor);
					}
					CreateFactorsCatalog(pathScheme, factorList);
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
