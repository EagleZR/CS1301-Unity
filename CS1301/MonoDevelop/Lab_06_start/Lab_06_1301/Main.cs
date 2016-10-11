using System;

namespace Lab_06_1301
{
	class MainClass
	{
		static void Main (string[] args)
		{
			Console.WriteLine ("One: ");
			whileTest (); // 10\n7\n4\n1\n

			Console.WriteLine ("\nTwo: ");
			doWhileTest (); //-42\n

			Console.WriteLine ("\nThree: ");
			//longWhileTest (); //Infinite loop?

			Console.WriteLine ("\nFour: ");
			forLoop1 (); //2 4 6 8 

			Console.WriteLine ("\nFive: ");
			forLoop2 (); //Hello 10\nHello 8\n Hello 6\nHello 4\nHello 2\n

			Console.WriteLine ("\nSix: ");
			while2 (); //0\n5\n10\n15\n

			Console.WriteLine ("\nSeven: ");
			forLoop3 (); //j = -3 output \n6\nj = 0 output 0\n\n

			Console.WriteLine ("\nWait, what?: ");
			doWhile2 (); //A is 5 B is -5

			Console.WriteLine ("\nReal Seven: ");
			Console.WriteLine ("Sum: " + seven ());

			Console.WriteLine ("\nEight: ");
			Console.WriteLine ("Sum of all odd numbers = " + eight ());

			Console.WriteLine ("\nNine: ");
			nine ();

			Console.WriteLine ("\nTen: ");
			Console.WriteLine ("Letter grade: " + ten ());
		}
		
		public static void whileTest ()
		{
			int x = 10;
			while (x > 0) 
			{
				Console.Out.WriteLine (x);
				x = x - 3;
			}
		}
		
		public static void doWhileTest ()
		{
			int x = -42;
			do {
				Console.Out.WriteLine (x);
				x = x - 3;
			} while (x > 0);
		}
		
		public static void longWhileTest ()
		{
			
			int x = 10;
			while (x > 0) 
			{
				Console.Out.WriteLine (x);
				x = x + 3;
			}
			Console.Out.WriteLine (x);
		}
		
		public static void forLoop1 ()
		{
			for (int count = 1; count < 5; count++)
				Console.Out.WriteLine ((2 * count) + " ");
			
		}
		
		public static void forLoop2 ()
		{
			for (int n = 10; n > 0; n = n - 2)
			{
				Console.Out.Write ("Hello ");
				Console.Out.WriteLine (n);
			}
			
		}
		
		public static void while2 ()
		{
			int k = 0;
			while (k <= 15) 
			{
				if (k % 5 == 0)
					Console.Out.WriteLine (k);
				k++;
			}
		}
		
		public static void forLoop3 ()
		{
			for (int j = -3; j < 3; j += 3) 
			{
				Console.Out.WriteLine ("j = " + j + " output ");
				if (j < 0)
					Console.Out.WriteLine (-2 * j);
				else
					Console.Out.WriteLine (j % 2);
				Console.Out.WriteLine ();
			}
		}
		
		public static void doWhile2 ()
		{
			int A = 5;
			int B = 90;
			do {
				B = (B / A) - 5;
				if (B > A)
					B = A + 30;
			} while (B >= 0);
			Console.Out.WriteLine ("A is " + A + " B is " + B);
			
		}

		public static int seven () {
			int total = 0;
			int input;
			while (true) {
				input = Int32.Parse (Console.ReadLine ());
				if (input % 2 == 0) {
					total += input;
				} else {
					return total;
				}
			}
		}

		public static int eight () {
			int total = 0;
			for (int i = 0; i <= 1000; i++) {
				if (i % 2 != 0) {
					total += i;
				}
			}
			return total;
		}

		public static void nine () {
			Random rand = new Random ();
			for (int i = 0; i < 10; i++) {
				//Random rand = new Random ();
				Console.WriteLine ("Random Number " + (i + 1) + ": " + rand.Next(20, 50)); 
			}
		}

		public static Char ten () {
			int grade = Int32.Parse (Console.ReadLine ());
			if (grade >= 90) {
				return 'A';
			} else if (grade >= 80) {
				return 'B';
			} else if (grade >= 70) { 
				return 'C';
			} else {
				return 'F';
			}
		}
	}
	
	
}

