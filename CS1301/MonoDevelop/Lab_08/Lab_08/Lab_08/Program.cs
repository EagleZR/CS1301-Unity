using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_08 {
	class Program {

		private enum Inputs { One = 1, Two, Three, Four }

		static void Main ( string[] args ) {
			Console.WriteLine( takeInputs() );
			Console.WriteLine( computerDiagnostic() + "\n" );
			ProductMethod();
			Console.WriteLine( "Press <Enter> to quit..." );
			Console.ReadLine();
		}

		public static String takeInputs () {
			Console.Write( "Please input a value of 1 through 4: " );
			int input = Int32.Parse( Console.ReadLine() );

			if (input == (int)Inputs.One) {
				return "Move left";
			} else if (input == (int) Inputs.Two) {
				return "Move right";
			} else if (input == (int) Inputs.Three) {
				return "Move up";
			} else if (input == (int) Inputs.Four) {
				return "Move down";
			} else {
				return "Invalid choice--turn skipped!!";
			}
		}

		public static String computerDiagnostic () {
			Console.Write( "Does the computer beep on startup? (Y/N): " );
			String input1 = Console.ReadLine();
			Console.Write( "Does the hard drive spin? (Y/N): " );
			String input2 = Console.ReadLine();
			bool beeps = false;
			bool spins = false;

			if ( input1.ToLower().Contains( "y" ) ) {
				beeps = true;
			}

			if ( input2.ToLower().Contains( "y" ) ) {
				spins = true;
			}

			if (beeps && spins) {
				return "Contact tech support.";
			} else if (beeps && !spins) {
				return "Check the drive contacts.";
			} else if (!beeps && !spins) {
				return "Bring the computer to the repair center.";
			} else {
				return "Check speaker contacts.";
			}
		}

		public static void ProductMethod () {
			Product p1 = new Product();
			Product p2 = new Product( "Peanut Butter", "Hormel", 2.99f, 16.1f );
			Console.WriteLine( p1 );
			Console.WriteLine( p2 );
			p1.Name = "Grape Jelly";
			p2.Name = "Orange Marmelade";
			p1.Price += 13f;
			p2.Price -= 1.25f;
			p2.Manufacturer = "PeterPan";
			p1.Manufacturer = "Skippy";
			Console.WriteLine( "\n" + p1 );
			Console.WriteLine( p2 );
		}
	}

	class Product {
		private String name;
		public String Name { get { return this.name; } set { this.name = value; } }

		private String manufacturer;
		public String Manufacturer { get { return this.manufacturer; } set { this.manufacturer = value; } }

		private float price;
		public float Price { get { return this.price; } set { this.price =  value > 0.0f ?  value : 0.0f; } }

		private float size;
		public float Size { get { return this.size; } set { this.size = value > 0.0f ? value : 0.0f; } }

		public Product () {
			this.Name = "";
			this.Manufacturer = "";
			this.Price = 0.0f;
			this.Size = 0.0f;
		}

		public Product (String name, String manufacturer, float price, float size ) {
			this.Name = name;
			this.Manufacturer = manufacturer;
			this.Price = price;
			this.Size = size;
		}

		public override string ToString () {
			return "Manufacturer: " + this.Manufacturer + "\nName: " + this.Name + "\nPrice: " + this.Price + "\nSize: " + this.Size;
		}
	}
}
