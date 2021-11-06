using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;


namespace OutputFileStructure
{
	public class InfoFromParusFile
	{
		private int _irregularVibration;
		private List<WorkSheetInfo> _workSheetInfos;

		public InfoFromParusFile(List<(string[], string, (int, int))> pathAndDislocation, string sectionName, ExcelPackage excelPackage)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			if(excelPackage.Workbook.Worksheets[0].Cells[2, 3].Value.ToString().Trim().ToLower() != sectionName.Trim().ToLower())
			{
				throw new Exception("Название сечений в базе данных и в файле ПАРУС не совпадают");
			}
			FindIrregularVibration(excelPackage);
		}

		public int IrregularVibration => _irregularVibration;

		private void FindIrregularVibration(ExcelPackage excelPackage)
		{
			string cellInfo = excelPackage.Workbook.Worksheets[0].Cells[2, 1].Value.ToString();
			string[] cellInfoSplit = cellInfo.Split(" ");
			_irregularVibration =  Int32.Parse(cellInfoSplit[2]);
		}
	}
}
