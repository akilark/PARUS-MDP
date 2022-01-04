using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using WorkWithCatalog;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс необходимый для формирования информации о группе ячеек
	/// </summary>
	public class WorkWithCellsGroup
	{
		private List<CellsGroup> _pathAndDislocation;

		/// <summary>
		/// Конструктор класса с 4 параметрами
		/// </summary>
		/// <param name="foldersPath">Путь к дереву папок</param>
		/// <param name="excelPackage">Эксель файл шаблона</param>
		/// <param name="FactorsInSample">факторы и расположение факторов в файле шаблона</param>
		/// <param name="schemes">Список схем</param>
		public WorkWithCellsGroup(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample, 
			List<Scheme> schemes)
		{
			_pathAndDislocation = new List<CellsGroup>();
			JuxtaposePathAndCells(foldersPath, excelPackage, FactorsInSample, schemes, false);
		}

		/// <summary>
		/// Конструктор класса с 5 параметрами
		/// </summary>
		/// <param name="foldersPath">Путь к дереву папок</param>
		/// <param name="excelPackage">Эксель файл шаблона</param>
		/// <param name="FactorsInSample">факторы и расположение факторов в файле шаблона</param>
		/// <param name="schemes">Список схем</param>
		/// <param name="Temperature">Рассматриваемые температуры</param>
		public WorkWithCellsGroup(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample, 
			List<Scheme> schemes, string[] Temperature)
		{
			_pathAndDislocation = new List<CellsGroup>();
			JuxtaposePathAndCells(foldersPath, excelPackage, FactorsInSample, schemes,true);
		}

		/// <summary>
		/// Свойство класса возвращающее список групп ячеек с соотнесенными путями 
		/// в файловой системе с указанием на необходимые файлы ПК ПАРУС
		/// </summary>
		public List<CellsGroup> PathAndDislocation => _pathAndDislocation;

		private void JuxtaposePathAndCells(string foldersPath, ExcelPackage excelPackage, List<(string, (int, int))> FactorsInSample,
			List<Scheme> schemes, bool temperatureDependence)
		{
			int substractor = temperatureDependence ? 2 : 1;
			int rowIndex;
			int nextRowIndex = FindNextRowForFactors(FactorsInSample[FactorsInSample.Count - substractor].Item2.Item1,
				FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2,excelPackage);
			bool flagFirst = true;
			while (true)
			{
				CellsGroup cellsGroup = new CellsGroup();
				rowIndex = nextRowIndex;
				
				if(!flagFirst)
				{
					nextRowIndex = FindNextRowForFactors(nextRowIndex,
						FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage);
					if (nextRowIndex == rowIndex)
					{
						break;
					}
				}
				flagFirst = false;
				
				
				cellsGroup.Factors = new List<(string, int)>();
				List<string> factors = new List<string>();
				for (int i = 0; i < FactorsInSample.Count - substractor + 1; i++)
				{
					string factorValue =
						excelPackage.Workbook.Worksheets[0].Cells[nextRowIndex, FactorsInSample[i].Item2.Item2].Value.ToString();
					if (factorValue != "-")
					{
						factors.Add("[" + factorValue + "]" + FactorsInSample[i].Item1);
						cellsGroup.Factors.Add((FactorsInSample[i].Item1, int.Parse(factorValue)));
					}
				}
				Permutation(factors.ToArray(), 0, ref factors);

				string schemeName = FindPreviousText(nextRowIndex, FactorsInSample[0].Item2.Item2 - 1, excelPackage);
				string direction = FindPreviousText(nextRowIndex, FactorsInSample[0].Item2.Item2 - 3, excelPackage);
				for(int i = 0; i < schemes.Count; i++)
				{
					if(schemes[i].SchemeName == schemeName)
					{
						cellsGroup.Disturbance = schemes[i].Disturbance;

					}
				}

				cellsGroup.SchemeName = schemeName;
				cellsGroup.Direction = direction;
				if(temperatureDependence)
				{
					cellsGroup.Temperature = int.Parse(FindPreviousText(nextRowIndex, FactorsInSample[FactorsInSample.Count - 1].Item2.Item2, excelPackage));
					cellsGroup.TemperatureDependence = true;
				}
				else
				{
					cellsGroup.TemperatureDependence = false;
				}
				string[] xlsxFilesFolder = FindFolderForCellsGroup(foldersPath, direction,
					FindPreviousText(nextRowIndex, FactorsInSample[0].Item2.Item2 - 2, excelPackage) + "_" + schemeName, factors);
				cellsGroup.Folders = xlsxFilesFolder;
				cellsGroup.StartID = (nextRowIndex, FactorsInSample[FactorsInSample.Count - 1].Item2.Item2 + 1);
				
				int size = FindNextRowForFactors(nextRowIndex,
					FactorsInSample[FactorsInSample.Count - substractor].Item2.Item2, excelPackage) - nextRowIndex -1;
				if(size == -1)
				{
					cellsGroup.SizeCellsArea = FindNextRowForFactors(nextRowIndex,
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
		/// Метод для нахождения следущей строки с текстом
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

		
		private void Permutation(string[] factors, int k, ref List<string> factorsMixed)
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
