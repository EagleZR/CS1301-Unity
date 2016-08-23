using System;

namespace Lab01
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Please input a number:");
			string input1 = Console.ReadLine ();
			int input1a = 0;
			while(!Int32.TryParse (input1, out input1a)){ // Still works if a word is submitted. Hmmmmm......
				Console.WriteLine ("That is not a number. Please input a number:"); //TODO does this work? YAY! :D
				input1 = Console.ReadLine ();
				input1a = 0;
			}

			Console.WriteLine ("Please input a word:");
			string input2 = Console.ReadLine ();

			TestObject testObject = new TestObject (input1a, input2);

			Console.Write ("You input the number ");
			Console.Write (testObject.getNumber ());
			Console.Write (" and the word ");
			Console.WriteLine (testObject.getWords ());
		}
	}
}