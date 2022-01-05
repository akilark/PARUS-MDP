using System;
using System.Collections.Generic;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Статический класс необходимый для сравнения различных величин
	/// </summary>
	public static class Comparator
	{
		/// <summary>
		/// Сравнение факторов
		/// </summary>
		/// <param name="factorsFromFolder">Факторы из дерева папок</param>
		/// <param name="factorsFromSample">Факторы из шаблона</param>
		/// <param name="temperatureDependence">Зависимость от температуры</param>
		/// <param name="temperature">Рассматриваемая температура</param>
		/// <returns></returns>
		public static List<(string, string[])> CompareFactors(FactorsWithDirection factorsFromFolder,
			List<(string, (int, int))> factorsFromSample, bool temperatureDependence, string[] temperature)
		{
			List<(string, string[])> factorList = new List<(string, string[])>();
			for (int i = 0; i < factorsFromSample.Count; i++)
			{
				bool addFlag = false;
				foreach ((string, string[]) factorFolder in factorsFromFolder.FactorNameAndValues)
				{
					if (factorsFromSample[i].Item1.ToLower().Trim() == factorFolder.Item1.ToLower().Trim())
					{
						factorList.Add(factorFolder);
						addFlag = true;
					}
				}
				if (temperatureDependence && i == factorsFromSample.Count - 1)
				{
					break;
				}
				if (!addFlag)
				{
					(string, string[]) emptyString = (factorsFromSample[i].Item1, new string[] { "-" });
					factorList.Add(emptyString);
				}
			}
			if (temperatureDependence)
			{
				(string, string[]) temperatureString = ("Температура", temperature);
				factorList.Add(temperatureString);
			}
			return factorList;
		}

		/// <summary>
		/// Сравнение строк (проверка идет в нижнем регистре и без пробелов)
		/// </summary>
		/// <param name="stringOne">первая сравниваемая строка</param>
		/// <param name="stringTwo">вторая сравниваемся строка</param>
		/// <returns></returns>
		public static bool CompareString(string stringOne, string stringTwo)
		{
			if(PrepareStringToCompare(stringOne) == PrepareStringToCompare(stringTwo))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Проверка содержит ли первая строка в себе вторую (проверка идет в нижнем регистре и без пробелов)
		/// </summary>
		/// <param name="stringOne">Строка в которую может входить вторая строка</param>
		/// <param name="stringTwo"> Строка, которая может входить в первую строку</param>
		/// <returns></returns>
		public static bool ContainsString(string stringOne, string stringTwo)
		{
			if(PrepareStringToCompare(stringOne).Contains(PrepareStringToCompare(stringTwo)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private static string PrepareStringToCompare(string stringInput)
		{
			string[] arrayInput = stringInput.Split(" ", StringSplitOptions.RemoveEmptyEntries);
			string stringOutput = "";
			foreach(string stringTmp in arrayInput)
			{
				stringOutput += stringTmp.ToLower();
			}
			return stringOutput;
		}
	}
}
