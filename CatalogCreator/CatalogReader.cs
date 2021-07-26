using System;
using System.Collections.Generic;
using System.IO;

namespace CatalogCreator
{
	public class CatalogReader
	{
		private string _path;
		private string _rootName;
		private string[] _allScheme;
		private List<(string, string[])> _factorsEast;
		private List<(string, string[])> _factorsWest;
		private List<(string, string[])> _factors;
		private string[] _directions = CatalogCreator.Directions;
		public bool _reverseable;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path">Путь к содержащий в себе корневую папку</param>
		public CatalogReader(string path)
		{
			_path = path;
			FindRootName(path);
			ReversableCheck(path);

		}

		private void FindRootName(string path)
		{
			_rootName = FolderName(path);
		}

		private void ReversableCheck(string path)
		{
			var directorysArray = Directory.GetDirectories(path);
			if (directorysArray.Length == 2)
			{
				if ((directorysArray[0].Contains(_directions[0]) && 
					directorysArray[1].Contains(_directions[1])))
				{
					_reverseable = true;
					_factorsWest = FindFactors(FindSchemeName(directorysArray[0]));
					_factorsEast = FindFactors(FindSchemeName(directorysArray[1]));
				}
			}
			else
			{
				_factors = FindFactors(FindSchemeName(path));
			}
		}

		private string FindSchemeName(string path)
		{
			var directorysArray = Directory.GetDirectories(path);
			_allScheme = new string[directorysArray.Length];
			for(int index = 0; index < directorysArray.Length; index++)
			{
				_allScheme[index] = FolderName(directorysArray[index]);
			}
			return directorysArray[0];
		}

		private List<(string, string[])> FindFactors(string schemePath)
		{
			var directorysArray = Directory.GetDirectories(schemePath);
			if (directorysArray.Length != 0)
			{
				var factorsTmp = new List<(string, string[])>();
				for (int i = 0; i < directorysArray.Length; i++)
				{
					directorysArray[i] = FolderName(directorysArray[i]);
				}
				var factorsArray = new List<string[]>();
				string[] factorsAmount = directorysArray[0].Split("_");
				foreach (string directoryString in directorysArray)
				{
					factorsArray.Add(directoryString.Split("_"));
				}
				for (int factorsIndex = 0; factorsIndex < factorsAmount.Length; factorsIndex++)
				{
					var factorValue = new string[0];
					var indexName = factorsArray[0][factorsIndex].IndexOf("]") + 1;
					var factorName = factorsArray[0][factorsIndex].Substring(indexName);
					for (int factorsValueIndex = 0; factorsValueIndex < 
						factorsArray.Count; factorsValueIndex++)
					{
						var uniqueValue = true;
						var indexValue = factorsArray[factorsValueIndex][factorsIndex].IndexOf("]");
						var factorValueTmp = 
							factorsArray[factorsValueIndex][factorsIndex].Substring(0, indexValue).Trim('[', ']');
						for (int z = 0; z < factorValue.Length; z++)
						{
							if (factorValueTmp == factorValue[z])
							{
								uniqueValue = false;
								break;
							}
						}
						if (uniqueValue)
						{
							Array.Resize(ref factorValue, factorValue.Length + 1);
							factorValue[factorValue.Length - 1] = factorValueTmp;
						}
					}
					factorsTmp.Add((factorName, factorValue));
				}
				return factorsTmp;
			}
			else
			{
				return null;
			}
			
		}
		private string FolderName(string path)
		{
			while (path.Contains(@"\"))
			{
				path = path.Substring(path.IndexOf(@"\") + 1);
			}
			return path;
		}
	}

}
