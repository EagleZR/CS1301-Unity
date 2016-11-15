using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_09 {
	public class Program {
		private static int[] integers = new int[20];
		private static int[] sortedIntegers = new int[20];

		public static void Main ( string[] args ) {
			GenerateRandoms();
			Console.WriteLine( "The array's contents are: " + ArrayToString() );
			Sort();
			Console.WriteLine( "The maximum value is: " + sortedIntegers[sortedIntegers.Count<int>() - 1] );
			Console.WriteLine( "The minimum value is: " + sortedIntegers[0] );
			int sum = Sum();
			Console.WriteLine( "The sum is: " + sum );
			Console.WriteLine( "The average is: " + sum / integers.Count<int>() );
			Console.Write( "Which value would you like to look for?: " );
			int value = Int32.Parse( Console.ReadLine() );
			Console.WriteLine( "The index of the value is: " + IndexOf( value ) );
			Console.ReadLine();
		}

		private static void GenerateRandoms () {
			Random rand = new Random();
			for ( int i = 0; i < integers.Count<int>(); i++ ) {
				integers[i] = rand.Next( -50, 101 );
			}
		}

		private static String ArrayToString () {
			string returnString = "";
			for ( int i = 0; i < integers.Count<int>(); i++ ) {
				returnString += integers[i] + ( i == integers.Count<int>() - 1 ? "" : ", " );
			}
			return returnString;
		}
		
		#region Sorting
		private static void Sort () {
			sortedIntegers = (int[])integers.Clone();
			sortedIntegers = Sort( sortedIntegers );
		}

		private static int[] Sort ( int[] array ) {
			if ( IsSorted( array ) ) {
				return array;
			} else {
				for ( int i = 0; i < array.Count<int>() - 1; i++ ) {
					if ( array[i] > array[i + 1] ) {
						int placeholder = array[i];
						array[i] = array[i + 1];
						array[i + 1] = placeholder;
					}
				}
				return Sort( array );
			}
		}

		private static bool IsSorted ( int[] array ) {
			for ( int i = 0; i < array.Count<int>() - 1; i++ ) {
				if ( array[i] > array[i + 1] ) {
					return false;
				}
			}
			return true;
		}
		#endregion

		private static int Sum () {
			int sum = 0;
			for ( int i = 0; i < integers.Count<int>(); i++ ) {
				sum += integers[i];
			}
			return sum;
		}

		private static int IndexOf ( int value ) {
			for ( int i = 0; i < integers.Count<int>(); i++ ) {
				if ( integers[i] == value ) {
					return i;
				}
			}
			return -1;
		}
	}
}
