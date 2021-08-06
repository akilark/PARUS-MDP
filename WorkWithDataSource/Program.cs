using System;
using System.Collections.Generic;

namespace WorkWithDataSource
{
	class Program
	{
		static void Main(string[] args)
		{
			PullData pull = new PullData();
			pull.PullFactors();
			pull.PullSchemes();
			pull.PullSections();

			List<string> sections = pull.Sections;

			//ChangeData pushSection = new ChangeData("Камала- Красноярская");
			//pushSection.Delete();

			ChangeData pushScheme = new ChangeData("Тест_1", "Нормальная схема", "ФОЛ Тест_5", 1);
			pushScheme.Insert();

			ChangeData pushFactors = new ChangeData("Тест_1", "На Запад", "Фактор Тест_2", "2");
			pushFactors.Insert();

		}
	}
}
