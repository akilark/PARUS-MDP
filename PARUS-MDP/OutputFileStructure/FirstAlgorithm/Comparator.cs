using System;
using System.Collections.Generic;
using System.Text;

namespace OutputFileStructure
{
	public static class Comparator
	{
	
		public static string[] CompareTemperature(string[] necessaryTemperatures, string[] existingTemperatures)
		{
			string[] compareTemperature = new string[0];
			foreach(string nTemperature in necessaryTemperatures)
			{
				foreach (string eTemperature in existingTemperatures)
				{
					if (nTemperature.Trim().ToLower() == eTemperature.Trim().ToLower())
					{
						Array.Resize(ref compareTemperature, compareTemperature.Length + 1);
						compareTemperature[compareTemperature.Length - 1] = nTemperature;
					}
				}
			}
			if(compareTemperature.Length != 0)
			{
				return compareTemperature;
			}
			else
			{
				throw new Exception("Нет вкладок для нужных температур в файлах Excel");
			}
		}

		public static List<(string, string[])> CompareFactors((string, (string, string[])[]) factorsFromFolder,
			List<(string, (int, int))> factorsFromSample, bool temperatureDependence, string[] temperature)
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
				(string, string[]) temperatureString = ("Температура", temperature);
				factorList.Add(temperatureString);
			}
			return factorList;
		}
	}
}
