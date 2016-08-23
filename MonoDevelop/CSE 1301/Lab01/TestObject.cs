using System;

	/* Author: Mark Zeagler
	 * Program Purpose: This program demonstrates some of the basic functions we learned in class.
	 * 					Specifically, it takes an input and stores it as an object. Then, it prints 
	 * 					out the object. 
	 */

namespace Lab01 //If you're importing this into your editor, make sure that this matches the name of the folder you put it in
{
	public class TestObject
	{
		/* -------------------------------   Attributes ----------------------------------
		 * These are where you store the attributes of the object. If it was a dog, here you could
		 * store its species, height, weight, gender, color, etc. 
		 */
		private int number;
		private String words;
		// ---------------------------- End Attributes -----------------------------------

		/* ---------------------------- Constructors -------------------------------------
		 * This is where you define the set-up of the object. Usually, this involves accepting 
		 * different kinds of creation parameters. For example, this object works only with numbers
		 * submitted as an integer. If you wanted to work with another type of number, say a double
		 * or a long, you may need to create a separate constructor for each of those.
		 */

		/* This is what is called an "Empty Constructor". This is not a requirement, but is often used
		 * for the purpose of turning a newly created object from being "null" to something that can be 
		 * as a placeholder. 
		 */ 
		public TestObject ()
		{
		}

		/* For my program, this is the only really useful constructor. As you can see, it takes the inputted
		 * parameters and stores them. You could also use this space to create new objects to be stored in 
		 * this one, run through some behaviors (e.g. character always makes a battle cry when created), or
		 * even start some strings. This object is simple, so all it needs to do is store some variables.
		 */
		public TestObject (int number, string setWords){
			/* "this.<variable>" is a very useful way to dictate which variable you're talking about. As you can
			 * see, there are two variables both called "number" in this method. First, there is the class 
			 * variable defined in the "Attributes" section, and secondly there is the inputted parameter.
			 * Use "this.<variable>" to specify that you are talking about the class variable, the attribute.
			 * 
			 * NOTE: If that's confusing, and usually it is at first, you can use a format like I did while 
			 * setting the words, where each one uses a different variable name. This works just as well, and 
			 * can help avoid confusion while you're getting started. 
			 */
			this.number = number;
			words = setWords;
		}
		// -------------------------- End Constructors -----------------------------------

		/* ----------------------------- Behavior ----------------------------------------
		 * This is where you store the processes and functions that this object does. You could do things like
		 * bark(), moveLeft(), toggleLandingMode(), kill(), etc. Below, all I need it for is the storage and
		 * retrieval of information, but you can get pretty advanced with it.
		 */
		public int getNumber(){
			return this.number;
		}

		public void setNumber(int newNumber){
			this.number = newNumber;
		}

		public string getWords(){
			return this.words;
		}

		public void setWords(int newWords){
			this.words = newWords;
		}
		// ------------------------------ End Behavior ------------------------------------
	}
}

