using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public static class TextDecor
	{
		public static void FactorCellsUnion(int row, int column,bool temperatureUse, int temperatureCount, ref ExcelPackage excelPackage)
		{
			if(temperatureUse)
			{
				if(excelPackage.Workbook.Worksheets[0].Cells[row, column + 1].Value != null)
				{
					while (excelPackage.Workbook.Worksheets[0].Cells[row, column].Value != null)
					{
						ChangeTextStyle(row, column, ref excelPackage);
						excelPackage.Workbook.Worksheets[0].Cells[row, column, row + temperatureCount - 1, column].Merge = true;
						row = row + temperatureCount;
					}
				}
			}
			else
			{
				/*
				while (excelPackage.Workbook.Worksheets[0].Cells[row, column].Value != null)
				{
					ChangeTextStyle(row, column, ref excelPackage);
					excelPackage.Workbook.Worksheets[0].Cells[row, column, row + temperatureCount - 1, column].Merge = true;
					row = row + temperatureCount;
				}
				*/
			}
			
		}

		public static void FirstCellsUnion(int row, int column, int amountFilledRows, int startRow, ref ExcelPackage excelPackage)
		{
			int nextTextIndex = FindNextTextInColumn(row, column, excelPackage);
			while (nextTextIndex != row)
			{
				RotateText(row, column, ref excelPackage);
				ChangeTextStyle(row, column, ref excelPackage);
				excelPackage.Workbook.Worksheets[0].Cells[row, column, nextTextIndex, column].Merge = true;
				row = nextTextIndex + 1;
				nextTextIndex = FindNextTextInColumn(row, column,excelPackage);
			}
			RotateText(row, column, ref excelPackage);
			ChangeTextStyle(row, column, ref excelPackage);
			excelPackage.Workbook.Worksheets[0].Cells[row, column, amountFilledRows + startRow - 1, column].Merge = true;
		}

		private static void RotateText(int row, int column, ref ExcelPackage excelPackage)
		{
			excelPackage.Workbook.Worksheets[0].Cells[row, column].Style.TextRotation = 90;
		}

		private static void ChangeTextStyle(int row, int column, ref ExcelPackage excelPackage)
		{
			excelPackage.Workbook.Worksheets[0].Cells[row, column].Style.Font.Name = "Times New Roman";
			excelPackage.Workbook.Worksheets[0].Cells[row, column].Style.Font.Size = 8;
			excelPackage.Workbook.Worksheets[0].Cells[row, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			excelPackage.Workbook.Worksheets[0].Cells[row, column].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
		}

		private static int FindNextTextInColumn(int row, int column, ExcelPackage excelPackage)
		{
			for (int i = row + 1; i < 1000; i++)
			{
				if (excelPackage.Workbook.Worksheets[0].Cells[i, column].Value != null)
				{
					return i - 1;
				}
			}
			return row;
		}
	}
}
