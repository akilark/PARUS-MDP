using System;
using System.IO;
using System.Collections.Generic;
using WorkWithDataSource;

namespace WorkWithCatalog
{
	/// <summary>
	/// Класс для тестов
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			/*
			CatalogReader CR = new CatalogReader(@"C:\test\Тест_1");
			FolderForCellsGroup FFCG = new FolderForCellsGroup(@"C:\test\Тест_1", CR.Factors[0].Item1, CR.AllScheme[0]);
			string[] xlsxFiles;
			for(int i = 3; i > 0; i--)
			{
				try
				{
					xlsxFiles = FFCG.Find("[1] Фактор Тест_"+i);
					break;
				}
				catch (ArgumentException)
				{

				}
			}
			
			PullData pull = new PullData("Тест_2");
			CatalogCreator catalog = new CatalogCreator(@"C:\test\Тест_2", pull.Factors, pull.Schemes);
			catalog.Create();

			//var CatalogRead = new CatalogReader(@"C:\test\Камала- Красноярская");
			
			*/
		}
	}
}
