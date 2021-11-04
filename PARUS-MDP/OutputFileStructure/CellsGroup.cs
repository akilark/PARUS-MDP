using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using WorkWithCatalog;

namespace OutputFileStructure
{
	public class CellsGroup
	{
		private string[] _temperature;
		private List<(string[], (int, int))> _pathAndDislocation;

		public CellsGroup(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample)
		{
			_pathAndDislocation = new List<(string[], (int, int))>();
			JuxtaposePathAndCells(foldersPath, excelPackage, FactorsInSample, false);
		}
		public CellsGroup(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample, string[] Temperature)
		{
			_pathAndDislocation = new List<(string[], (int, int))>();
			_temperature = Temperature;
			JuxtaposePathAndCells(foldersPath, excelPackage, FactorsInSample, true);
		}
		public List<(string[], (int, int))> PathAndDislocation => _pathAndDislocation;

		private void JuxtaposePathAndCells(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample,
			bool temperatureDependence)
		{
			int substractor = temperatureDependence ? 2 : 1;
			int rowIndex = 0;
			int nextRowIndex = findNextRowForFactors(FactorsInSample[FactorsInSample.Count - substractor].Item2.Item1,
				FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2,excelPackage);
			while (true)
			{
				rowIndex = nextRowIndex;
				if (temperatureDependence)
				{
					int indexTmp = findNextRowForFactors(rowIndex,
						FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage);
					nextRowIndex = (indexTmp - rowIndex) * _temperature.Length + rowIndex;
				}
				else
				{
					nextRowIndex = findNextRowForFactors(rowIndex,
						FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage);
				}
				if(nextRowIndex == rowIndex)
				{
					break;
				}

				List<string> factors = new List<string>();
				for (int i = 0; i < FactorsInSample.Count - substractor + 1; i++)
				{
					string factorValue =
						excelPackage.Workbook.Worksheets[0].Cells[rowIndex, FactorsInSample[i].Item2.Item2].Value.ToString();
					if (factorValue != "-")
					{
						factors.Add("[" + factorValue + "]" + FactorsInSample[i].Item1);
					}
				}
				Permutation(factors.ToArray(), 0, ref factors);

				string[] xlsxFilesFolder = FindFolderForCellsGroup(foldersPath,
					FindPreviousText(rowIndex, FactorsInSample[0].Item2.Item2 - 3, excelPackage),
					FindPreviousText(rowIndex, FactorsInSample[0].Item2.Item2 - 2, excelPackage) + "_" +
					FindPreviousText(rowIndex, FactorsInSample[0].Item2.Item2 - 1, excelPackage),
					factors);
				_pathAndDislocation.Add((xlsxFilesFolder,(rowIndex, FactorsInSample[FactorsInSample.Count - 1].Item2.Item2 + 1)));
			}
		}

		private string[] FindFolderForCellsGroup(string folderPath, string directon, string schemeNameWithNumber, List<string> factorsMixed)
		{
			FolderForCellsGroup folderForCellsGroup = new FolderForCellsGroup(folderPath, directon, schemeNameWithNumber);
			foreach(string factorMix in factorsMixed)
			{
				try
				{
					return folderForCellsGroup.Find(factorMix);
				}
				catch (ArgumentException)
				{

				}
			}
			throw new Exception($"Таких факторов нет в {folderPath}");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="excelPackage"></param>
		/// <returns>Номер следующей после startIndex строки с текстом, 
		/// если строка последняя возвращает startIndex</returns>
		private int findNextRowForFactors(int startIndex, int columnIndex, ExcelPackage excelPackage)
		{

			for (int i = startIndex + 1 ; i < startIndex + 100; i++)
			{
				if(excelPackage.Workbook.Worksheets[0].Cells[i, columnIndex].Value != null)
				{
					if (excelPackage.Workbook.Worksheets[0].Cells[i, columnIndex].Value.ToString() != "")
					{
						return i;
					}
				}
				
			}
			return startIndex;
		}

		private string FindPreviousText(int rowStartIndex, int columnIndex, ExcelPackage excelPackage)
		{
			for (int i = rowStartIndex; i > 0; i--)
			{
				if (excelPackage.Workbook.Worksheets[0].Cells[i, columnIndex].Value != null)
				{
					return excelPackage.Workbook.Worksheets[0].Cells[i, columnIndex].Value.ToString();
				}
			}
			throw new IndexOutOfRangeException($"Неправильное значение rowStartIndex, лист закончился") ;
		}

		
		public void Permutation(string[] factors, int k, ref List<string> factorsMixed)
		{
			if(k >= factors.Length)
			{
				factorsMixed.Add(string.Join("_", factors));
			}
			else
			{
				Permutation(factors, k + 1, ref factorsMixed);
				for(int i = k + 1; i < factors.Length; i++)
				{
					Swap(ref factors[k], ref factors[i]);
					Permutation(factors, k + 1, ref factorsMixed);
					Swap(ref factors[k], ref factors[i]);
				}
			}
		}

		private void Swap(ref string itemOne, ref string itemTwo)
		{
			string temp = itemOne;
			itemOne = itemTwo;
			itemTwo = temp;
		}
		 
	}
}
