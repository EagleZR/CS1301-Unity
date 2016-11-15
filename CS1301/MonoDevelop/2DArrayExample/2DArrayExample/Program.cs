using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DArrayExample {
	class Program {
		static void Main ( string[] args ) {
			MultipleCoordinates myCoordinates = new MultipleCoordinates( 4, 3 );

			int[] coordinates = { 0, 0, 0 };
			myCoordinates.AddCoordinates ( coordinates);

			coordinates[0] = 1; coordinates[1] = 8; coordinates[2] = 9;
			myCoordinates.AddCoordinates( coordinates );

			coordinates[0] = 10; coordinates[1] = 4; coordinates[2] = 6;
			myCoordinates.AddCoordinates( coordinates );

			coordinates[0] = -5; coordinates[1] = 3; coordinates[2] = 5;
			myCoordinates.AddCoordinates( coordinates );

			Console.WriteLine( myCoordinates );
			Console.ReadLine();
		}
	}
}
