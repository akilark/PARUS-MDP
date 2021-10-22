using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public class FactorsCombinations
	{
		private string[] _factorsMixed;
		private List<string> _temperature;

		public FactorsCombinations((string, (string, string[])[]) factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample)
		{
			_factorsMixed = GenerateFactorMatrix(CompareFolderAndSample(factorsFromFolder, factorsFromSample, false));
		}

		public FactorsCombinations((string, (string, string[])[]) factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample, List<string> temperature)
		{
			_temperature = temperature;
			_factorsMixed = GenerateFactorMatrix(CompareFolderAndSample(factorsFromFolder, factorsFromSample, true));
		}

		public string[] FactorMixed => _factorsMixed;

		private List<(string, string[])> CompareFolderAndSample((string, (string, string[])[]) factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample, bool temperatureDependence)
		{
			List<(string, string[])> factorList = new List<(string, string[])>();
			for (int i = 0; i < factorsFromSample.Count; i++)
			{
				bool addFlag = false;
				foreach ((string, string[]) factorFolder in factorsFromFolder.Item2)
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
				string[] temperatureArray = new string[_temperature.Count];
				for (int i = 0; i < _temperature.Count; i++)
				{
					temperatureArray[i] = _temperature[i];
				}

				(string, string[]) temperatureString = ("Температура", temperatureArray);
				factorList.Add(temperatureString);
			}
			return factorList;
		}

		private string[] GenerateFactorMatrix(List<(string, string[])> factors)
		{
			var amountFactorValues = AmountFactorsValueCalculate(factors);
			var factorsMixedSize = 1;
			for (int i = 0; i < amountFactorValues.Length; i++)
			{
				factorsMixedSize *= amountFactorValues[i];
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
						factorsMixed[areaIndex] = factors[currentFactor].Item2[factorIndex];
					}
					else
					{
						factorsMixed[areaIndex] = factorsMixed[areaIndex] +
							"_" + factors[currentFactor].Item2[factorIndex];
					}
					counterUnitInArea++;
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
