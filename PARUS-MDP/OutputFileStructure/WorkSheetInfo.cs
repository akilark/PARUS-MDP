using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class WorkSheetInfo
	{
		private string[] _results;

		public WorkSheetInfo(int workSheetNumber, string repairScheme, ExcelPackage excelPackage)
		{
			Inizialize();
			int startRow = FindScheme(repairScheme, workSheetNumber, excelPackage) + 1;
			FindInfo(startRow, workSheetNumber, excelPackage);
		}
		public WorkSheetInfo(int workSheetNumber, ExcelPackage excelPackage)
		{
			Inizialize();
			int startRow = 9;
			FindInfo(startRow, workSheetNumber, excelPackage);
		}
		public string[] AssignedValues => _results;

		private void Inizialize()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_results = new string[7];
		}

		private int FindScheme(string repairScheme, int workSheetNumber, ExcelPackage excelPackage)
		{
			string[] textSeparators = new string[] { "ремонт ", "ремонта " };
			repairScheme.Trim();
			string schemeName = repairScheme;
			foreach (string textSeparator in textSeparators)
			{
				string[] repairSchemeSplit = repairScheme.ToLower().Split(textSeparator);
				if(schemeName.Length > repairSchemeSplit[repairSchemeSplit.Length - 1].Length)
				{
					schemeName = repairSchemeSplit[repairSchemeSplit.Length - 1];
				}
			}

			for(int i = 9; i < 1000; i++)
			{
				object CellValue = excelPackage.Workbook.Worksheets[workSheetNumber].Cells[i, 1].Value;
				if(CellValue != null)
				{
					if (CellValue.ToString().Contains("Схема ремонта"))
					{
						if (CellValue.ToString().ToLower().Contains(schemeName))
						{
							return i;
						}
					}
				}
				
			}
			throw new Exception($"Схемы {schemeName} на листе {excelPackage.Workbook.Worksheets[workSheetNumber].Name} нет");
		}

		private void FindInfo(int startRow, int workSheetNumber, ExcelPackage excelPackage)
		{
			string[] columnsName = 
				new string[] { "Рсеч-Рно, МВт", "Перегружаемый элемент", "Рпред*0,8-Pно", "Р(Uкр/0.85)-Рно", "Узел", "Рпред" };
			var workSheet = excelPackage.Workbook.Worksheets[workSheetNumber];
			for(int column = 0; column< columnsName.Length; column ++ )
			{
				for (int i= 2; i<100; i++)
				{
					if (workSheet.Cells[startRow, i].Value != null)
					{
						if (workSheet.Cells[startRow, i].Value.ToString() == columnsName[column])
						{
							if(workSheet.Cells[startRow + 1, i].Value == null)
							{
								throw new Exception($"Данные в ячейке {workSheet.Cells[startRow + 1, i]} отсутствуют");
							}
							
							_results[column] = ParseRoundAndMultiply(workSheet.Cells[startRow + 1, i].Value.ToString(), 1);
							break;
						}
					}
				}
			}
			_results[_results.Length - 1] = ParseRoundAndMultiply(_results[_results.Length - 2], 0.92);
		}

		private string ParseRoundAndMultiply(string text, double multiplier)
		{
			double number;
			if(double.TryParse(text,out number))
			{
				return Math.Round(Math.Round(number) * multiplier).ToString();
			}
			else
			{
				return text;
			}
			
		}
	}
}
