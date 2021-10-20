using System;
using System.Collections.Generic;
using System.Text;
using WorkWithCatalog;
using System.IO;
using System.Reflection;
using OfficeOpenXml;


namespace OutputFileStructure
{
	public class Sample
	{
		private CatalogReader _catalogReader;
		private string _samplePath;
		private string _path;
		private ExcelPackage _excelPackage;
		private int _startRow;
		private bool _temperatureDependence;

		public Sample(string path, string samplePath)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_path = path;
			_samplePath = samplePath;
			DownloadSample();
			_catalogReader = new CatalogReader(_path);
			_startRow = FindStartRow();
			FillDirection();
			SaveSampleWithStructure(_excelPackage);
			
		}

		private void DownloadSample()
		{
			FileInfo fileInfo = new FileInfo(_samplePath);
			_excelPackage = new ExcelPackage(fileInfo);

		}

		private int FindFirstOccurance()
		{
			for (int i = 1; i < 100; i++)
			{
				if (_excelPackage.Workbook.Worksheets[0].Cells[i, 1].Value != null)
				{
					return i;
				}
			}
			throw new Exception("Файл шаблона должен быть заполнен");
		}

		private int FindStartRow()
		{
			int startRow = FindFirstOccurance();
			while (true)
			{
				startRow = startRow + 1;
				if (_excelPackage.Workbook.Worksheets[0].Cells[startRow, 1].Value == null)
				{
					if(!_excelPackage.Workbook.Worksheets[0].Cells[startRow, 1].Merge)
					{
						return startRow;
					}
				}
			}
		}


		private void FillDirection()
		{
			int columnNumberForDirection = 1;
			int rowNumberForDirection = _startRow;

			for (int i = 0; i < _catalogReader.Factors.Count; i++)
			{
				
				string[] factorList = GenerateFactorMatrix(CompareFolderAndSample(_catalogReader.Factors, i));
				
				_excelPackage.Workbook.Worksheets[0].Cells[rowNumberForDirection, columnNumberForDirection].Value = _catalogReader.Factors[i].Item1;
				rowNumberForDirection = FillScheme(columnNumberForDirection + 1, rowNumberForDirection, factorList);
			}	
		}


		private int FillScheme(int columnNumberForScheme, int rowNumberForScheme, string[] factors)
		{
			foreach (string scheme in _catalogReader.AllScheme)
			{
				string numberScheme = scheme.Substring(0, scheme.IndexOf("_"));
				string nameScheme = scheme.Substring(scheme.IndexOf("_") + 1);
				_excelPackage.Workbook.Worksheets[0].Cells[rowNumberForScheme, columnNumberForScheme].Value = numberScheme;
				_excelPackage.Workbook.Worksheets[0].Cells[rowNumberForScheme, columnNumberForScheme + 1].Value = nameScheme;
				
				int rowNumberForFactor = rowNumberForScheme;

				rowNumberForScheme = FillFactors(columnNumberForScheme + 2, rowNumberForScheme, factors);

			}
			return rowNumberForScheme + 1;
		}

		private int FillFactors(int columnNumberForFactors, int rowNumberForFactor, string[] factors)
		{
			foreach (string factorRow in factors)
			{
				string[] factorsToCell = factorRow.Split("_");
				for(int i=0; i< factorsToCell.Length;i++)
				{
					_excelPackage.Workbook.Worksheets[0].Cells[rowNumberForFactor, columnNumberForFactors + i].Value = factorsToCell[i];
				}
				rowNumberForFactor = rowNumberForFactor + 1;
			}
			return rowNumberForFactor;
		}

		private List<(string, string[])> CompareFolderAndSample(List<(string, (string, string[])[])> factorsWithDirection, int indexDirection)
		{
			List<(string, (int, int))> factorsFromSample = FactorsInSample();
			List<(string, string[])> factorList = new List<(string, string[])>();
			foreach ((string, (int, int)) factorSample in factorsFromSample)
			{
				bool addFlag = false;
				foreach((string,string[]) factorFolder in _catalogReader.Factors[indexDirection].Item2)
				{
					if (factorSample.Item1.ToLower().Trim() == factorFolder.Item1.ToLower().Trim())
					{
						factorList.Add(factorFolder);
						addFlag = true;
					}
				}
				if (!addFlag)
				{
					(string, string[]) emptyString = (factorSample.Item1, new string[] { "-" });
					factorList.Add(emptyString);
				}
			}
			if (_temperatureDependence)
			{
				string[] temperatureArray = new string[_catalogReader.Temperature.Count];
				for (int i = 0; i < _catalogReader.Temperature.Count; i++)
				{
					temperatureArray[i] = _catalogReader.Temperature[i];
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
			//переделать убрать метод и сделать всё через item
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
						factorsMixed[areaIndex] =  factors[currentFactor].Item2[factorIndex] ;
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



		private List<(string, (int,int))> FactorsInSample()
		{
			int rowWithData = FindFirstOccurance();
			int rowWithoutData = _startRow;
			int startIndex = 0;
			int endIndex = 0;
			int rowWithFactors = 0;
			for(int columnIndex = 1; columnIndex < 100; columnIndex++)
			{
				if (_excelPackage.Workbook.Worksheets[0].Cells[rowWithData, columnIndex].Value.ToString().ToLower().Contains("схема"))
				{
					startIndex = columnIndex + 1;
					break;
				}
			}
			for (int columnIndex = startIndex; columnIndex < 100; columnIndex++)
			{
				if (!_excelPackage.Workbook.Worksheets[0].Cells[rowWithoutData - 1, columnIndex].Merge)
				{
					endIndex = columnIndex - 1;
					break;
				}
			}
			for (int rowIndex = rowWithData; rowIndex < 100; rowIndex++)
			{
				if (_excelPackage.Workbook.Worksheets[0].Cells[rowIndex, endIndex].Value != null &&
					!_excelPackage.Workbook.Worksheets[0].Cells[rowIndex, endIndex].Value.ToString().ToLower().Contains("влияющи"))
				{
					rowWithFactors = rowIndex;
					break;
				}
			}
			List<(string, (int, int))> outputFactors = new List<(string, (int, int))>();

			for(int columnIndex = startIndex; columnIndex <= endIndex; columnIndex ++)
			{
				outputFactors.Add((_excelPackage.Workbook.Worksheets[0].Cells[rowWithFactors, columnIndex].Value.ToString(), (rowWithFactors, columnIndex))); 
			}
			return outputFactors;
		}

		private void SaveSampleWithStructure(ExcelPackage excelPackage)
		{
			FileInfo file = new FileInfo(_path + @$"\Сформированная структура.xlsx");
			excelPackage.SaveAs(file);
		}
	}
}
