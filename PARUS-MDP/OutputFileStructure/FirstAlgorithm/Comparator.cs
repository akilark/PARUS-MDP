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
