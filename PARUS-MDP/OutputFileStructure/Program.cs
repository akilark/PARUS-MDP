using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using DataTypes;
using WorkWithDataSource;

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
			PullData pullData = new PullData("Тест_1");
			WorkWithCellsGroup wwcg = new WorkWithCellsGroup(@"C:\test\Тест_1", excelPackage2, sampleSection.FactorsInSample(),
				pullData.Schemes,new string[] { "35", "30", "25", "20", "15" });
			List<CellsGroup> cellsGroups = new List<CellsGroup>();
			for (int i = 0; i< 5; i ++)
			{
				cellsGroups.Add(wwcg.PathAndDislocation[i]);
			}
			SampleControlActions sampleControlActions = new SampleControlActions(excelPackage);


			string[] directions = new string[] { "На запад", "На восток" };

			var controlActionWithNeedDirection = sampleControlActions.ControlActionsForNeedDirection(directions[0]);

			var compare = new CompareControlActions(pullData.Imbalances, controlActionWithNeedDirection, pullData.AOPOlist, pullData.AOCNlist, true);

			InfoFromParusFile infoFromParusFile = new InfoFromParusFile(cellsGroups, compare.Imbalances, compare.AOPOlist, compare.AOCNlist, compare.LAPNYlist, 
				false, ref excelPackage2);


			FileInfo file = new FileInfo(@$"C:\test\Тест_1\Сформированная структура2.xlsx");
			excelPackage2.SaveAs(file);
			
		}
	}
}
