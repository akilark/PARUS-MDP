using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace CatalogCreator
{
	public class Catalog
	{
		private string _path;
		private string _rootName;
		public bool _reverseable = true;
		private string[] _repairScheme;
		private string[] _allScheme;
		private List<(string, string[])> _factorsEast;
		private List<(string, string[])> _factorsWest;
		private List<(string, string[])> _factors;
		private string _directionEast = "На восток";
		private string _directionsWest = "На запад";
		public bool _flagDoubleRepair = true;
		public bool _flagRepair = true; //Если только нормальная схема - false

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

		public bool Reverseable
		{
			get
			{
				return _reverseable;
			}
			set
			{
				_reverseable = value;
			}
		}
		public string[] RepairScheme
		{
			get
			{
				return _repairScheme;
			}
			set
			{
				_repairScheme = value;
			}
		}
		public List<(string, string[])> FactorsEast
		{
			get
			{
				return _factorsEast;
			}
			set
			{
				_factorsEast = value;
			}
		}

		public List<(string, string[])> FactorsWest
		{
			get
			{
				return _factorsWest;
			}
			set
			{
				_factorsWest = value;
			}
		}

		public List<(string, string[])> Factors
		{
			get
			{
				return _factors;
			}
			set
			{
				_factors = value;
			}
		}
		


		public void Create()
		{
			string pathRoot = Path.Combine(_path, _rootName);
			Directory.CreateDirectory(pathRoot);

			CreateReversable(pathRoot, new string[] {_directionsWest,_directionEast});
		}

		private void CreateReversable(string pathRoot, string[] direction)
		{
			if(Reverseable == true)
			{
				foreach (string direct in direction)
				{
					string pathReversable = Path.Combine(pathRoot, direct);
					Directory.CreateDirectory(pathReversable);
					CreateScheme(pathReversable);
				}
			}
			else
			{
				CreateScheme(pathRoot);
			}
		}

		private void ExpandSchemeArray()
		{
			if (_flagRepair)
			{
				if (_flagDoubleRepair)
				{
					var doubleRepair = new DoubleRepair(RepairScheme);
					var repairTmp = new string[RepairScheme.Length + doubleRepair.DoubleRepairSchemeName.Length + 1];
					repairTmp[0] = "Нормальная схема";
					RepairScheme.CopyTo(repairTmp, 1);
					doubleRepair.DoubleRepairSchemeName.CopyTo(repairTmp, RepairScheme.Length + 1);
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

		private void CreateScheme(string pathReversable)
		{
			int serialNumber = 1;
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
					CreateFactorsCatalog(pathScheme, Factors);
				}
			}
		}

		private void DirectionFactors(string pathScheme)
		{
			if (pathScheme.Contains(_directionEast))
			{
				CreateFactorsCatalog(pathScheme, FactorsEast);
			}
			else if (pathScheme.Contains(_directionsWest))
			{
				CreateFactorsCatalog(pathScheme, FactorsWest);
			}
			else
			{
				CreateFactorsCatalog(pathScheme, Factors);
			}
		}

		private void CreateFactorsCatalog(string pathScheme, List<(string, string[])> factors)
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
						CreateOneFactor(pathScheme, factors);
						break;
					}
					default:
					{
						CreateFewFactors(pathScheme, factors);
						break;
					}
				}
			}
		}
		
		private void CreateFewFactors(string pathScheme, List<(string, string[])> factors)
		{
			foreach(string stringMiexedFactors in GenerateMixedFactors(factors))
			{
				string pathFactor = Path.Combine(pathScheme, stringMiexedFactors);
				Directory.CreateDirectory(pathFactor);
			}
		}

		private string[] GenerateMixedFactors(List<(string, string[])> factors)
		{
			var amountFactorValues = new int[0];
			foreach ((string, string[]) factor in factors)
			{
				Array.Resize(ref amountFactorValues, amountFactorValues.Length + 1);
				amountFactorValues[amountFactorValues.Length - 1] = factor.Item2.Length;
				
			}
			int sizeFactorsMixed = 1;

			for(int i= 0; i < amountFactorValues.Length; i++)
			{
				sizeFactorsMixed = sizeFactorsMixed * amountFactorValues[i];
			}

			string[] factorsMixed = new string[sizeFactorsMixed];
			int delimer = 1;
			for(int currentFactor = 0; currentFactor < factors.Count; currentFactor++)
			{
				int valueTimer = 0;
				
				delimer = delimer * amountFactorValues[currentFactor];
				for (int currentIndex = 0; currentIndex < sizeFactorsMixed; currentIndex++)
				{
					switch(currentFactor)
					{
						case 0:
							{
								if(currentIndex < sizeFactorsMixed / delimer)
								{
									factorsMixed[currentIndex] = "[" + factors[currentFactor].Item2[0] + "]" + factors[currentFactor].Item1;
								}
								else if (currentIndex >= sizeFactorsMixed / delimer)
								{
									factorsMixed[currentIndex] = "[" + factors[currentFactor].Item2[1] + "]" + factors[currentFactor].Item1;
								}
								break;
							}
						default:
							{
								if (valueTimer == sizeFactorsMixed * amountFactorValues[currentFactor - 1] / delimer )
								{
									valueTimer = 0;
								}
								if (valueTimer < sizeFactorsMixed / delimer)
								{
									valueTimer++;
									factorsMixed[currentIndex] = factorsMixed[currentIndex] + "_[" + factors[currentFactor].Item2[0] + "]" + factors[currentFactor].Item1;
								}
								else if (valueTimer >= sizeFactorsMixed / delimer)
								{
									valueTimer++;
									factorsMixed[currentIndex] = factorsMixed[currentIndex] + "_[" + factors[currentFactor].Item2[1] + "]" + factors[currentFactor].Item1;
								}
								break;
							}
					}
				}
			}
			return factorsMixed;
		}


		private void CreateOneFactor(string pathScheme, List<(string, string[])> factors)
		{
			foreach (string factorValue in factors[0].Item2)
			{
				string pathFactor = Path.Combine(pathScheme, FolderNameFormat(factorValue, " ", factors[0].Item1,""));
				Directory.CreateDirectory(pathFactor);
			}
		}

		private string FolderNameFormat(string firstWord, string delimiter, string secondWord, string secondDilimiter)
		{
			return "[" + firstWord + "]" + delimiter + secondWord + secondDilimiter;
		}
	}
}
