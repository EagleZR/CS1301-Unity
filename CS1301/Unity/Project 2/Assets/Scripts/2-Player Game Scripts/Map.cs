using System;
using UnityEngine;

namespace AssemblyCSharp
{
	

	public class Map
	{
		/*
		public class Position {

			private int x, y;
			private char data;

			public Position () {
				x = 0;
				y = 0;
				data = "#";
			}

			public Position (int setX, int setY) {
				x = setX;
				y = setY;
				data = '#';
			}

			public Position (int setX, int setY, char setData) {
				x = setX;
				y = setY;
				data = setData;
			}
		}*/

		private int levels;
		private int width;
		private int depth;

		private char[, ,] map;

		public Map () {
			levels = 5;
			width = 100;
			depth = 100;

			map = new char[levels, width, depth];

			for (int i = 0; i < levels; i++) {
				for (int u = 0; u < width; u++) {
					for (int o = 0; o < depth; o++) {
						map[i, u, o] = '#';
					}
				}
			}
		}

		public Map (int numLevels, int setWidth, int setDepth) {
			levels = numLevels;
			width = setWidth;
			depth = setDepth;

			map = new char[levels, width, depth];

			for (int i = 0; i < levels; i++) {
				for (int u = 0; u < width; u++) {
					for (int o = 0; o < depth; o++) {
						map[i, u, o] = '#';
					}
				}
			}
		}

		// Checks to see if the given level has been fully mapped.
		// Might work in a system to make sure each tank gets to map each level. 
		public bool levelMapped (int checkLevel) {
			for (int i = 0; i < width; i++) {
				for (int u = 0; u < depth; u++) {
					if (map [checkLevel, i, u] == '#') {
						return false;
					}
				}
			}
			return true;
		}

		// @setXPos and @setYPos are given in scene coordinates, and converted to the map coordinates
		public void setPosition (int setLevel, int setXPos, int setYPos, char setData) {
			setXPos += 50;
			setYPos += 50;
			map [setLevel, setXPos, setYPos] = setData;
		}

		/*public Vector3[] findPath (Vector3 startLocation, Vector3 destination) {

		}*/

		// public char[][] findPath (

	}
}

