using System;

	/* Author: Mark Zeagler
	 * Program Purpose: This program demonstrates some of the basic functions we learned in class.
	 * 					Specifically, it takes an input and stores it as an object. Then, it prints 
	 * 					out the object. 
	 */

namespace Lab01 //If you're importing this into your editor, make sure that this matches the name of the folder you put it in
{
	class MainClass
	{
		/* This is where the program begins. This class works with the TestObject.cs class to store 
		 * information that is then retrieved again by this class for output. This program is broken 
		 * into 4 main parts:
		 * 
		 * 1. Read Number
		 * 2. Read Word
		 * 3. Create Object
		 * 4. Print Object Information
		 * 
		 * NOTE: string[] args is something you can use to input start-up arguments that can be used to 
		 * change how the program runs. For example, if the program uses multi-threading, and you 
		 * wanted to limit how many threads it would use. This is a little more advanced feature, and 
		 * you likely won't use this feature in this class at all. However, the argument still needs to 
		 * be set as a parameter for any Main method.
		 */

		public static void Main (string[] args)
		{
			/* 1. Read Number
			 * 
			 * This reads in a number to be stored in the object. Since all inputs are read as strings,
			 * this needs to be verified as an integer first. That is accomplished using the loop. The mechanics
			 * of the loop aren't really too important, I actually had to look it up on stackoverflow.com, but 
			 * the reason behind it is extremely important; you don't want to send a string to be stored in an
			 * int, that's how you get a runtime error.
			 */
			Console.WriteLine ("Please input a number:");
			string input1 = Console.ReadLine ();
			int input1a = 0;
			while(!Int32.TryParse (input1, out input1a)){ 
				Console.WriteLine ("That is not a number. Please input a number:"); 
				input1 = Console.ReadLine ();
				input1a = 0;
			}

			/* 2. Read Word
			 * 
			 * This reads in a word from the second input. Since anything can be put here, there is no need 
			 * to test for validity.
			 */
			Console.WriteLine ("Please input a word:");
			string input2 = Console.ReadLine ();

			/* 3. Create Object
			 * 
			 * This one may seem straightforward, but there is a lot going on under the scenes. First,
			 * The object's class matches the input paramaters to the correct constructor. Next, the constructor
			 * runs through it's defined creation steps. Ok, maybe it's not too complicated in this case, but
			 * it can easily be made so.
			 */
			TestObject testObject = new TestObject (input1a, input2);

			/* 4. Print Object
			 * 
			 * This one is almost always very simple. Assuming that the object's .toString() method is set up
			 * correctly, that's all that needs to be called. In this case, it is not, but it isn't an issue.
			 * Each of the stored variables are retrieved and printed out in a format that's easy to read.
			 * 
			 * NOTE: You can string in object behaviors into the creation of the string. This can be helpful when
			 * the information to be output is unknown or likely to change. For example, this time, it is based on
			 * whatever information you input during runtime. Another example may be if the program is trying to 
			 * output the name of the person in the class with the highest grade, or a list of people with A's, or 
			 * any number of similar instances. You don't need to use it, and can work around it as I show commented
			 * out below.
			 */
			string outputText = "You input the number " + testObject.getNumber () + " and the word " + testObject.getWords ();
			Console.WriteLine (outputText);

			// Console.Write ("You input the number ");
			// Console.Write (testObject.getNumber());
			// Console.Write (" and the word ");
			// Console.WriteLine (testObject.getWords());
		}
	}
}