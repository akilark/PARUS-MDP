using System;
using System.IO;
using System.Collections.Generic;
using WorkWithDataSource;

namespace CatalogCreator
{
	/// <summary>
	/// Класс для тестов
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			PullData pull = new PullData("Тест_1");
			pull.PullFactors();
			pull.PullSchemes();
			CatalogCreator catalog = new CatalogCreator(@"C:\test", "Тест_1", pull.Factors, pull.Shemes);
			catalog.Create();
			
			//var CatalogRead = new CatalogReader(@"C:\test\Камала- Красноярская");
			/*
			//TEST 1 Создание каталога без ремонтных схем
			
			CatalogCreator catalogCreator = new CatalogCreator();

			catalogCreator.DirectoryPath = @"C:\test";
			catalogCreator.RootName = "Камала- Красноярская";
			catalogCreator.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator.FactorsWest = new List<(string, string[])> {
				("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}), 
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }),
			("Потребление", new string[] { "2222", "4121" })};
			catalogCreator.FactorsEast = new List<(string, string[])> {
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }) };
			catalogCreator._flagDoubleRepair = true;
			catalogCreator._flagRepair = false;
			catalogCreator._reverseable = true;

			catalogCreator.Create();

			//TEST 2 Создание каталога без реверсивного перетока
			CatalogCreator catalogCreator2 = new CatalogCreator();

			catalogCreator2.DirectoryPath = @"C:\test\TEstestetstetstetset\JSAfafasfjkasflasfka;sfasfasfasfasf";
			catalogCreator2.RootName = "2Камала- Красноярская";
			catalogCreator2.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator2.Factors = new List<(string, string[])> {
				("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}),
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" })};
			catalogCreator2._flagDoubleRepair = true;
			catalogCreator2._flagRepair = true;
			catalogCreator2._reverseable = false;

			catalogCreator2.Create();


			//TEST 3 Создание каталога без двойных ремонтов

			CatalogCreator catalogCreator3 = new CatalogCreator();

			catalogCreator3.DirectoryPath = @"C:\test\TEstestetstetstetset\JSAfafasfjkasflasfka;sfasfasfasfasf";
			catalogCreator3.RootName = "3Камала- Красноярская";
			catalogCreator3.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator3.FactorsWest = new List<(string, string[])> {
				("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}),
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" })};
			catalogCreator3.FactorsEast = new List<(string, string[])> {
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }) };
			catalogCreator3._flagDoubleRepair = false;
			catalogCreator3._flagRepair = true;
			catalogCreator3._reverseable = true;

			catalogCreator3.Create();


			//TEST 4 создание каталога с 3-мя влияющими факторами

			CatalogCreator catalogCreator4 = new CatalogCreator();

			catalogCreator4.DirectoryPath = @"C:\test\TEstestetstetstetset\JSAfafasfjkasflasfka;sfasfasfasfasf";
			catalogCreator4.RootName = "4Камала- Красноярская";
			catalogCreator4.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator4.FactorsWest = new List<(string, string[])> {
				("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}),
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }),
				("Потребление центрального Красноярского энергоузла", new string[] { "2000 МВт", "2222 МВт" })};

			catalogCreator4.FactorsEast = new List<(string, string[])> {
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }) };
			catalogCreator4._flagDoubleRepair = true;
			catalogCreator4._flagRepair = true;
			catalogCreator4._reverseable = true;

			catalogCreator4.Create();


			//TEST 5 создание каталога с 4 значениями влияющего фактора

			CatalogCreator catalogCreator5 = new CatalogCreator();

			catalogCreator5.DirectoryPath = @"C:\test\TEstestetstetstetset\JSAfafasfjkasflasfka;sfasfasfasfasf";
			catalogCreator5.RootName = "5Камала- Красноярская";
			catalogCreator5.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator5.FactorsWest = new List<(string, string[])> {
				("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}),
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4", "6", "8" })};
			catalogCreator5.FactorsEast = new List<(string, string[])> {
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }) };
			catalogCreator5._flagDoubleRepair = false;
			catalogCreator5._flagRepair = true;
			catalogCreator5._reverseable = true;

			catalogCreator5.Create();


			//TEST 6 Создание каталога без влияющих факторов но с созданием list не реверсивный переток

			CatalogCreator catalogCreator6 = new CatalogCreator();

			catalogCreator6.DirectoryPath = @"C:\test";
			catalogCreator6.RootName = "6Камала- Красноярская";
			catalogCreator6.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator6.Factors = new List<(string, string[])> {};
			catalogCreator6._flagDoubleRepair = true;
			catalogCreator6._flagRepair = true;
			catalogCreator6._reverseable = false;

			catalogCreator6.Create();

			//TEST 7 Создание каталога без влияющих факторов, без создания List реверсивный переток

			CatalogCreator catalogCreator7 = new CatalogCreator();

			catalogCreator7.DirectoryPath = @"C:\test";
			catalogCreator7.RootName = "7Камала- Красноярская";
			catalogCreator7.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator7._flagDoubleRepair = true;
			catalogCreator7._flagRepair = true;
			catalogCreator7._reverseable = true;

			catalogCreator7.Create();

			//TEST 8 создание каталога с 4-мя влияющими факторами

			CatalogCreator catalogCreator8 = new CatalogCreator();

			catalogCreator8.DirectoryPath = @"C:\test\";
			catalogCreator8.RootName = "8Камала- Красноярская";
			catalogCreator8.RepairScheme = new string[] {"Ремонт Камала-1- Красноярская",
				"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
				"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"};
			catalogCreator8.FactorsWest = new List<(string, string[])> {
				("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}),
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }),
				("Потребление центрального Красноярского энергоузла", new string[] { "2000 МВт", "2222 МВт", "444МВт" })};

			catalogCreator8.FactorsEast = new List<(string, string[])> {
				("Количество генераторов Красноярская ГЭС-220 кВ", new string[] { "2", "4" }) };
			catalogCreator8._flagDoubleRepair = true;
			catalogCreator8._flagRepair = true;
			catalogCreator8._reverseable = true;

			catalogCreator8.Create();
			*/

		}
	}
}
