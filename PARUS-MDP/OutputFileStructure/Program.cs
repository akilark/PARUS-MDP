using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OfficeOpenXml;

namespace OutputFileStructure
{
	class Program
	{
		static void Main(string[] args)
		{
			SampleSection sample = new SampleSection(@"C:\test\Тест_1", @"C:\test\Тест_1\Шаблон для теста2.xlsx", new string[] {"20","15","10","5"});
			FileInfo fileInfo = new FileInfo(@"C:\test\Тест_1\Сформированная структура.xlsx");
			var excelPackage = new ExcelPackage(fileInfo);
			CellsGroup CG = new CellsGroup(@"C:\test\Тест_1\", excelPackage, sample.FactorsInSample(),
			new string[] { "20", "15", "10", "5" });
			var pad = CG.PathAndDislocation;
			Console.WriteLine(pad[0].Item1);
		}
	}
}
