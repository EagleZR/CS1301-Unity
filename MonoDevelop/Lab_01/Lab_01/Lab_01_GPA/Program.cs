using System;

namespace Lab_01
{
	class MainClass
	{
		public static void Main(string[] unused){
			double GPA;

			// Read GPA input
			Console.WriteLine ("Please input your GPA:");
			string input1 = Console.ReadLine (); // So an inputted word doesn't crash it
			while (!Double.TryParse (input1, out GPA)) { 
				Console.WriteLine ("That is not a number. Please input your GPA as a number:"); 
				input1 = Console.ReadLine ();
			}

			// Output category
			if (GPA > 3.5) {
				Console.WriteLine ("Congratulations! You will graduate with honors!\n");
			} else if (GPA < 2.0) {
				Console.WriteLine ("I am sorry, you are on probation.\n");
			} else {
				Console.WriteLine ("Thank you.\n");
			}
		}
	}
}

