using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public class FactorsCombinations
	{
		private string[,] _factorsMixed;
		private string[] _temperature;

		public FactorsCombinations((string, (string, string[])[]) factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample, int temperatureMerge)
		{
			_factorsMixed = GenerateFactorMatrix(CompareFolderAndSample(factorsFromFolder, factorsFromSample, false), temperatureMerge);
		}

		public FactorsCombinations((string, (string, string[])[]) factorsFromFolder, 
			List<(string, (int, int))> factorsFromSample, string[] temperature, int temperatureMerge)
		{
			_temperature = temperature;
			_factorsMixed = GenerateFactorMatrix(CompareFolderAndSample(factorsFromFolder, factorsFromSample, true), temperatureMerge);
		}

		public string[,] FactorMixed => _factorsMixed;

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
				(string, string[]) temperatureString = ("Температура", _temperature);
				factorList.Add(temperatureString);
			}
			return factorList;
		}

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
						factorsMixed[addedLines + i, currentFactor] = factors[currentFactor].Item2[factorIndex];
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
