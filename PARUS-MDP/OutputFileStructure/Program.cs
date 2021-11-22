using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using OutputFileStructure.DataTypes;

namespace OutputFileStructure
{
	class Program
	{
		static void Main(string[] args)
		{
			
			SampleSection sampleSection = new SampleSection(@"C:\test\Тест_1", @"C:\test\Тест_1\Шаблон для теста2.xlsx", new string[] {"35", "30", "25", "20", "15" });
			FileInfo fileInfo = new FileInfo(@"C:\test\Тест_1\Шаблон для теста2.xlsx");
			var excelPackage = new ExcelPackage(fileInfo);
			FileInfo fileInfo2 = new FileInfo(@"C:\test\Тест_1\Сформированная структура.xlsx");
			var excelPackage2 = new ExcelPackage(fileInfo2);
			WorkWithCellsGroup wwcg = new WorkWithCellsGroup(@"C:\test\Тест_1", excelPackage2, sampleSection.FactorsInSample(), new string[] { "35", "30", "25", "20", "15" });
			List<CellsGroup> cellsGroups = new List<CellsGroup>();
			for (int i = 0; i< 5; i ++)
			{
				cellsGroups.Add(wwcg.PathAndDislocation[i]);
			}
			SampleControlActions sampleControlActions = new SampleControlActions(excelPackage);

			var imbalancesDataSource = new List<ImbalanceDataSource>();

			InfoFromParusFile infoFromParusFile = new InfoFromParusFile(cellsGroups, sampleControlActions.ImbalanceInSample[0].Item2,
				imbalancesDataSource, sampleControlActions.LAPNYinSample[0].Item2, ref excelPackage2);


			FileInfo file = new FileInfo(@$"C:\test\Тест_1\Сформированная структура2.xlsx");
			excelPackage2.SaveAs(file);
			
		}
	}
}
