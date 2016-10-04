using System;

namespace Lab_05
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Phone phone1 = new Phone ("Galaxy S5", "Samsung", "Mark", "Black", 250.0, 4.7, new Battery ("Samsung Battery", 200, 8));

			Phone phone2 = new Phone ("iPhone 7", "Apple", "White", 4.2, new Battery (250, 10));

			Console.WriteLine ("Phone 1: \n" + phone1);
			Console.WriteLine ("Phone 2: \n" + phone2);
		}
	}
}
