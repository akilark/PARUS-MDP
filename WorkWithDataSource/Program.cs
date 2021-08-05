using System;

namespace WorkWithDataSource
{
	class Program
	{
		static void Main(string[] args)
		{
			PullData pull = new PullData("Камала- Красноярская");
			pull.PullSections();
			pull.PullFactors();
			pull.PullScheme();

			ChangeData push = new ChangeData("Камала- Шамала");
			push.Direction="На восток";
			push.Factor = "Рыбов";
			push.FactorValue = "15";
			push.Insert();
		

		}
	}
}
