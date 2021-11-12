using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;


namespace OutputFileStructure
{
	public class InfoFromParusFile
	{
		private int _nonRegularOscilation;

		public InfoFromParusFile(CellsGroup cellsGroup, List<ControlAction> sampleControlActions, ref ExcelPackage excelPackageOutputFile)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			foreach (string path in cellsGroup.Folders)
			{
				FileInfo fileInfo = new FileInfo(path);
				var excelPackage = new ExcelPackage(fileInfo);
				FindNonRegularOscilation(excelPackage);
				for (int i = 0; i <  excelPackage.Workbook.Worksheets.Count; i++)
				{
					WorkSheetInfo workSheetInfoTMP = new WorkSheetInfo(cellsGroup.SchemeName, NonRegularOscilation,
						sampleControlActions, excelPackage.Workbook.Worksheets[i]);
					//Вставка текста в нужные строки и объединение
				}
			}
			
			
		}

		public int NonRegularOscilation => _nonRegularOscilation;

		private void FindNonRegularOscilation(ExcelPackage excelPackage)
		{
			string cellInfo = excelPackage.Workbook.Worksheets[0].Cells[2, 1].Value.ToString();
			string[] cellInfoSplit = cellInfo.Split(" ");
			_nonRegularOscilation =  Int32.Parse(cellInfoSplit[2]);
		}

		private void InsertText( string value, (int,int) rowAndColumn, ref ExcelPackage excelPackageOutputFile)
		{
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Value = value;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.Font.Name = "Times New Roman";
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.Font.Size = 8;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
		}

		private void MergeColumn((int, int) columnMerge, int column, ref ExcelPackage excelPackageOutputFile)
		{
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[columnMerge.Item1, column, columnMerge.Item2 - 1, column].Merge = true;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[columnMerge.Item1, column, columnMerge.Item2 - 1, column].
					Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
		}

		private void NeedForControl(AllowPowerOverflows allowPowerOverflows, (int,int) StartID, int SizeCellsArea)
		{

		}
	}
}
