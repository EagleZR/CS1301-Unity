using System;

namespace Lab_01
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			string name;
			int age;
			int weight;
			int height;

			// Read Name
			Console.WriteLine ("Please input your name: ");
			name = Console.ReadLine ();

			// Read Age
			// int age = Int32.Parse(Console.ReadLine();
			Console.WriteLine ("Please input your age:");
			string input1 = Console.ReadLine (); // So an inputted word doesn't crash it
			while(!Int32.TryParse (input1, out age)){ 
				Console.WriteLine ("That is not a number. Please input your age as a number:"); 
				input1 = Console.ReadLine ();
			}

			// Read Weight
			Console.WriteLine ("Please input your weight:");
			input1 = Console.ReadLine (); // So an inputted word doesn't crash it
			while(!Int32.TryParse (input1, out weight)){ 
				Console.WriteLine ("That is not a number. Please input your weight as a number:"); 
				input1 = Console.ReadLine ();
			}

			// Read Height
			Console.WriteLine ("Please input your height:");
			input1 = Console.ReadLine (); // So an inputted word doesn't crash it
			while(!Int32.TryParse (input1, out height)){ 
				Console.WriteLine ("That is not a number. Please input your height as a number:"); 
				input1 = Console.ReadLine ();
			}

			// Calculate BMI
			double bmi = (double) weight / (double)(height * height) * 703;

			Console.WriteLine("Here is the information you input: \nname: " + name + "\nage: " + age + "\nweight: " + weight + "\nheight: " + height + "\nUsing this information, your Body Mass Index has been calculated to be: " + bmi + "\n");
		}
	}
}
