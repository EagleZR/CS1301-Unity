using System;

namespace Lab_07
{
	public class Course
	{

		public string description;
		public int courseNumber;
		public int courseHours;
		public string prefix;

		// Empty Constructor
		public Course ()
		{
		}

		public Course (String setDescription, int setCourseNumber, int setCourseHours, string setPrefix) {
			description = setDescription;
			courseNumber = setCourseNumber;
			courseHours = setCourseHours;
			prefix = setPrefix;
		}

		public override String ToString(){
			if (prefix == null) {
				return "Course not initialized.";
			} else {
				return prefix + courseNumber + " \nCourse Hours: " + courseHours + "\nCourse Description: " + description;
			}
		}
	}
}

