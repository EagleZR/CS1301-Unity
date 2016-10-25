using System;
using System.Collections.Generic;

namespace Lab_07 {
	
	class MainClass {
		static List <Course> myList = new List <Course> ();
		
		public static void Main (string[] args) {
			myList.Add (new Course ("name1", 1, 3, "One - "));
			myList.Add (new Course ("name2", 2, 3, "Two - "));
			myList.Add (new Course ("name3", 3, 3, "Three - "));
			myList.Add (new Course ("name4", 4, 3, "Four - "));

			inputLoop ();
		}

		private static void inputLoop () {
			while (true) {
				Console.Write ("----------------------------------\nInput the following commands \n1 - Add a new Course.\n2 - Remove a Course.\n3 - Print all Courses.\n0 - End Program.\n----------------------------------\nPlease input your selection: ");
				int input = Int32.Parse (Console.ReadLine ());
				if (input == 1) {
					Console.WriteLine ("----------------------------------\nAdding a course...");
					// Add Course
					Console.Write ("Please input the description of the course you would like to add: ");
					string description = Console.ReadLine ();
					Console.Write ("Please input the course number (no letters): ");
					int courseNumber = Int32.Parse (Console.ReadLine ());
					Console.Write ("Please input the course hours: ");
					int courseHours = Int32.Parse (Console.ReadLine ());
					Console.Write ("Please input the course code (letters before the numbers): ");
					string courseCode = Console.ReadLine ();

					Course newCourse = new Course (description, courseNumber, courseHours, courseCode);

					Console.WriteLine ("\nThis is the course you have created.\n" + newCourse.ToString () + "\n\nPlease confirm that this is correct (Y/N): ");
					string confirmation = Console.ReadLine ();
					if (confirmation.ToLower ().Contains ("y")) {
						Console.WriteLine ("Adding Course...");
						myList.Add (newCourse);
					} else {
						Console.WriteLine ("Discarding new Course...");
					}
				} else if (input == 2) {
					/*
					Console.WriteLine ("----------------------------------\nRemoving a Course...");
					// Remove Course
					Console.Write ("Please input the course number (no letters) of the course you would like to remove: ");
					int courseNumber = Int32.Parse(Console.ReadLine ());
					Console.Write ("Please input the course code (letters before the numbers) of the course you would like to remove: ");
					string courseCode = Console.ReadLine ();

					Course removalCourse = findCourseByName (courseNumber, courseCode);

					if (removalCourse == null) {
						Console.WriteLine ("\nThe Course you requested to remove is not in the list.");
					} else {
						Console.WriteLine ("\nPlease confirm that you would like to remove the course listed below.\n" + removalCourse.ToString () + "\nPlease confirm (Y/N): ");
						string confirmation = Console.ReadLine ();
						if (confirmation.ToLower ().Contains ("y")) {
							Console.WriteLine ("RemovingCourse Course...");
							myList.Remove (removalCourse);
						} else {
							Console.WriteLine ("Keeping the Course...");
						}
					}
					*/ 
					Console.WriteLine ("----------------------------------\nRemoving a Course...");
					// Remove Course
					Console.Write ("Please input the index of the course you would like to remove (Note: The first item in the list is at index '0'): ");
					int index = Int32.Parse (Console.ReadLine ());
					Console.WriteLine ("\nThe course you selected is listed below.\n" + myList[index].ToString () + "\nPlease confirm you wish to remove it (Y/N): ");
					string confirmation = Console.ReadLine ();
					if (confirmation.ToLower ().Contains ("y")) {
						Console.WriteLine ("RemovingCourse Course...");
						myList.RemoveAt (index);
					} else {
						Console.WriteLine ("Keeping the Course...");
					}
				} else if (input == 3) {
					Console.WriteLine ("----------------------------------\nThe Courses contained in the List are shown below: ");
					// Print Courses
					foreach (Course currCourse in myList) {
						Console.WriteLine (currCourse.ToString () + "\n");
					}
				} else {
					Console.WriteLine ("Ending Program.");
					break;
				}
			}
		}

		private static Course findCourseByName (int courseNumber, string courseCode) {
			foreach (Course currCourse in myList) {
				if (currCourse.courseNumber == courseNumber && currCourse.prefix.Contains(courseCode)) {
					return currCourse;
				}
			}
			return null;
		}
	}
}
