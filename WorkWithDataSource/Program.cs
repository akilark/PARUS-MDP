using System;
using System.Collections.Generic;

namespace WorkWithDataSource
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
			pull.PullSections();

			List<string> sections = pull.Sections;

			//ChangeData pushSection = new ChangeData("Камала- Красноярская");
			//pushSection.Delete();

			ChangeData pushScheme = new ChangeData("Тест_2", "Ремонт Тест_3", "ФОЛ Тест_1", true);
			pushScheme.Insert();

			ChangeData pushFactors = new ChangeData("Тест_2", "на Восток", "Фактор Тест_2", "2");
			pushFactors.Insert();

		}
	}
}
