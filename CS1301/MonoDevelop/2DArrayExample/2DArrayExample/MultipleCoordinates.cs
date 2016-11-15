using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DArrayExample {
	class MultipleCoordinates {
		private int[,] coordinatesCollection;

		/* coordinatesCollection = new int [4, 3] example shown below
		 * 
		 * coordinatesCollection [0, ] = { x, y, z}
		 *		coordinatesCollection [0, 0] = x
		 *		coordinatesCollection [0, 1] = y
		 *		coordinatesCollection [0, 2] = z
		 *		
		 *	coordinatesCollection [1, ] = { x, y, z}
		 *		coordinatesCollection [1, 0] = x
		 *		coordinatesCollection [1, 1] = y
		 *		coordinatesCollection [1, 2] = z
		 *		
		 *	coordinatesCollection [2, ] = { x, y, z}
		 *		coordinatesCollection [2, 0] = x
		 *		coordinatesCollection [2, 1] = y
		 *		coordinatesCollection [2, 2] = z
		 *		
		 *	coordinatesCollection [3, ] = { x, y, z}
		 *		coordinatesCollection [3, 0] = x
		 *		coordinatesCollection [3, 1] = y
		 *		coordinatesCollection [3, 2] = z
		 */

		private int numCoordinates;
		private int sizeFilled;
		private int numAxes;

		public MultipleCoordinates (int numCoordinates, int numAxes) {
			this.coordinatesCollection = new int[numCoordinates, numAxes];
			this.numCoordinates = numCoordinates;
			this.sizeFilled = 0;
			this.numAxes = numAxes;
		}

		public bool AddCoordinates ( int[] coordinates ) {
			if (this.sizeFilled < this.numCoordinates) {
				for (int i = 0; i < this.numAxes; i++ ) {
					this.coordinatesCollection[this.sizeFilled, i] = coordinates[i];
				}
				this.sizeFilled++;
				return true;
			} else {
				return false;
			}
		}

		public override string ToString () {
			string returnString = "{";
			for (int i = 0; i < this.numCoordinates;  i ++ ) {
				returnString += "{";
				for (int u = 0; u < this.numAxes; u++) {
					returnString += this.coordinatesCollection[i, u] + ( u == this.numAxes - 1 ? "" : ", " );
				}
				returnString += "}" + ( i == this.numCoordinates - 1 ? "" : ", " );
			}
			returnString += "}";
			return returnString;
		}
	}
}
