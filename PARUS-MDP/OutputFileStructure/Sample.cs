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

		public Sample(string path, string samplePath, bool temperatureDependence)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_path = path;
			_samplePath = samplePath;
			_temperatureDependence = temperatureDependence;
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
			int amountFilledRows = 0;
			for (int i = 0; i < _catalogReader.Factors.Count; i++)
			{
				FactorsCombinations factorsCombinations;
				if (_temperatureDependence)
				{
					factorsCombinations = new FactorsCombinations(_catalogReader.Factors[i], 
						FactorsInSample(), _catalogReader.Temperature);
				}
				else
				{
					factorsCombinations = new FactorsCombinations(_catalogReader.Factors[i],
						FactorsInSample());
				}
				string[] factorList = factorsCombinations.FactorMixed;
				amountFilledRows = amountFilledRows + factorList.Length * _catalogReader.AllScheme.Length;
				_excelPackage.Workbook.Worksheets[0].Cells[rowNumberForDirection, columnNumberForDirection].Value = _catalogReader.Factors[i].Item1;
				rowNumberForDirection = FillScheme(columnNumberForDirection + 1, rowNumberForDirection, factorList);
			}

			List<(string, (int, int))> factorsInSample = FactorsInSample();

			for (int i = 0; i< factorsInSample[0].Item2.Item2 - columnNumberForDirection; i++)
			{
				TextDecor.FirstCellsUnion(_startRow, columnNumberForDirection + i,
					amountFilledRows, _startRow, ref _excelPackage);
			}

			for (int i = 0 ; i < factorsInSample.Count; i++) 
			{
				TextDecor.FactorCellsUnion(_startRow, factorsInSample[0].Item2.Item2 + i,
					_temperatureDependence, _catalogReader.Temperature.Count, ref _excelPackage);
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
			return rowNumberForScheme;
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
