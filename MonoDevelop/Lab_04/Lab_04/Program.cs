using System;

namespace Lab_04
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Course course1 = new Course ();
			Course course2 = new Course ("An introductory programming class.", 1301, 4, "CSE ");

			Console.WriteLine (course1);
			Console.WriteLine (course2);
		}
	}
}
