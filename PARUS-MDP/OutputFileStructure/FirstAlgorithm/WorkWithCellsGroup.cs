using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using WorkWithCatalog;

namespace OutputFileStructure
{
	public class WorkWithCellsGroup
	{
		private string[] _temperature;
		private List<CellsGroup> _pathAndDislocation;

		public WorkWithCellsGroup(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample, 
			List<(string, (string, bool)[])> schemes)
		{
			_pathAndDislocation = new List<CellsGroup>();
			JuxtaposePathAndCells(foldersPath, excelPackage, FactorsInSample, schemes, false);
		}
		public WorkWithCellsGroup(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample, 
			List<(string, (string, bool)[])> schemes, string[] Temperature)
		{
			_pathAndDislocation = new List<CellsGroup>();
			_temperature = Temperature;
			JuxtaposePathAndCells(foldersPath, excelPackage, FactorsInSample, schemes,true);
		}
		public List<CellsGroup> PathAndDislocation => _pathAndDislocation;

		private void JuxtaposePathAndCells(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample,
			List<(string, (string, bool)[])> schemes, bool temperatureDependence)
		{
			int substractor = temperatureDependence ? 2 : 1;
			int rowIndex;
			int nextRowIndex = FindNextRowForFactors(FactorsInSample[FactorsInSample.Count - substractor].Item2.Item1,
				FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2,excelPackage);
			while (true)
			{
				CellsGroup cellsGroup = new CellsGroup();
				rowIndex = nextRowIndex;
				
				nextRowIndex = FindNextRowForFactors(rowIndex,
					FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage);
				
				if(nextRowIndex == rowIndex)
				{
					break;
				}
				cellsGroup.Factors = new List<(string, int)>();
				List<string> factors = new List<string>();
				for (int i = 0; i < FactorsInSample.Count - substractor + 1; i++)
				{
					string factorValue =
						excelPackage.Workbook.Worksheets[0].Cells[rowIndex, FactorsInSample[i].Item2.Item2].Value.ToString();
					if (factorValue != "-")
					{
						factors.Add("[" + factorValue + "]" + FactorsInSample[i].Item1);
						cellsGroup.Factors.Add((FactorsInSample[i].Item1, int.Parse(factorValue)));
					}
				}
				Permutation(factors.ToArray(), 0, ref factors);

				string schemeName = FindPreviousText(rowIndex, FactorsInSample[0].Item2.Item2 - 1, excelPackage);
				string direction = FindPreviousText(rowIndex, FactorsInSample[0].Item2.Item2 - 3, excelPackage);
				for(int i = 0; i < schemes.Count; i++)
				{
					if(schemes[i].Item1 == direction)
					{
						for(int j = 0; j < schemes[i].Item2.Length; j++)
						{
							if(schemes[i].Item2[j].Item1 == schemeName)
							{
								cellsGroup.AutomaticForScheme = schemes[i].Item2[j].Item2;
							}
						}
					}
				}

				cellsGroup.SchemeName = schemeName;
				cellsGroup.Direction = direction;
				if(temperatureDependence)
				{
					cellsGroup.Temperature = int.Parse(FindPreviousText(rowIndex, FactorsInSample[FactorsInSample.Count - 1].Item2.Item2, excelPackage));
				}
				//TODO добавть Try catch.
				string[] xlsxFilesFolder = FindFolderForCellsGroup(foldersPath, direction,
					FindPreviousText(rowIndex, FactorsInSample[0].Item2.Item2 - 2, excelPackage) + "_" + schemeName,
					factors);
				cellsGroup.Folders = xlsxFilesFolder;
				cellsGroup.StartID = (rowIndex, FactorsInSample[FactorsInSample.Count - 1].Item2.Item2 + 1);
				
				int size = FindNextRowForFactors(nextRowIndex,
					FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage) - nextRowIndex -1;
				if(size == 0)
				{
					cellsGroup.SizeCellsArea = FindNextRowForFactors(rowIndex,
					FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage) - rowIndex - 1;
				}
				else
				{
					cellsGroup.SizeCellsArea = size;
				}

				_pathAndDislocation.Add(cellsGroup);
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
		private int FindNextRowForFactors(int startIndex, int columnIndex, ExcelPackage excelPackage)
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
