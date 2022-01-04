using System;
using System.Collections.Generic;
using System.Text;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс необходимый для комбинирования факторов для заполнения файла шаблона
	/// </summary>
	public class FactorsCombinations
	{
		private string[,] _factorsMixed;
		
		/// <summary>
		/// Конструктор с 3 параметрами
		/// </summary>
		/// <param name="factorsFromFolder">Факторы из директории</param>
		/// <param name="factorsFromSample"> Факторы из шаблона</param>
		/// <param name="temperatureMerge">Сколько строчек должна занимать одна
		/// ячейка для температуры (или крайний фактор, если зависимости от температуры нет)</param>
		public FactorsCombinations(FactorsWithDirection factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample, int temperatureMerge)
		{
			_factorsMixed = GenerateFactorMatrix(Comparator.CompareFactors(factorsFromFolder, factorsFromSample, false ,new string[0]), temperatureMerge);
		}

		/// <summary>
		/// Конструктор с 4 параметрами
		/// </summary>
		/// <param name="factorsFromFolder">Факторы из директории</param>
		/// <param name="factorsFromSample"> Факторы из шаблона</param>
		/// <param name="temperature">Массив рассматриваемых температур</param>
		/// <param name="temperatureMerge">Сколько строчек должна занимать одна
		/// ячейка для температуры (или крайний фактор, если зависимости от температуры нет)</param>
		public FactorsCombinations(FactorsWithDirection factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample, string[] temperature, int temperatureMerge)
		{
			_factorsMixed = GenerateFactorMatrix(Comparator.CompareFactors(factorsFromFolder, factorsFromSample, true, temperature), temperatureMerge);
		}

		/// <summary>
		/// Перемешанные факторы для заполнения файла шаблона
		/// </summary>
		public string[,] FactorMixed => _factorsMixed;


		private string[,] GenerateFactorMatrix(List<(string, string[])> factors, int temperatureMerge)
		{
			var amountFactorValues = AmountFactorsValueCalculate(factors);
			var factorsMixedSize = 1;

			for (int i = 0; i < amountFactorValues.Length; i++)
			{
				
				factorsMixedSize *= amountFactorValues[i];
			}
			var areaSize = factorsMixedSize;
			factorsMixedSize *= temperatureMerge;
			var factorsMixed = new string[factorsMixedSize,factors.Count];
			var delimer = 1;
			
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
				int addedLines = 0;
				while(factorsMixedSize >= addedLines + temperatureMerge)
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

					for(int i = 0; i < temperatureMerge; i++)
					{
						if(i == 0)
						{
							factorsMixed[addedLines + i, currentFactor] = factors[currentFactor].Item2[factorIndex];
						}
						else
						{
							factorsMixed[addedLines + i, currentFactor] = "";
						}
						
					}
						
					counterUnitInArea++;
					addedLines = addedLines + temperatureMerge;
				}
			}
			return factorsMixed;
		}

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
	}
}
