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

		}
	}
}
