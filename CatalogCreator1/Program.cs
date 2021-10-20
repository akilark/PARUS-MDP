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
			CatalogReader CR = new CatalogReader(@"C:\test\Тест_3");
			//PullData pull = new PullData("Тест_2");
			//pull.PullFactors();
			//pull.PullSchemes();
			//	CatalogCreator catalog = new CatalogCreator(@"C:\test", "Тест_2", pull.Factors, pull.Shemes);
			//catalog.Create();

			//var CatalogRead = new CatalogReader(@"C:\test\Камала- Красноярская");
			

		}
	}
}
