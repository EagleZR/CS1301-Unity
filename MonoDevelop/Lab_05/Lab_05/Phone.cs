using System;

namespace Lab_05
{
	public class Phone
	{
		private String model, manufacturer, owner, color;

		public String Model {
			get {
				return model;
			}
			set { 
				model = value;
			}
		}

		public String Manufacturer {
			get {
				return manufacturer;
			}
			set { 
				manufacturer = value;
			}
		}

		public String Owner {
			get {
				return owner;
			}
			set { 
				owner = value;
			}
		}

		public String Color {
			get {
				return color;
			}
			set { 
				color = value;
			}
		}
			
		private double price, screenSize;

		public double Price {
			get {
				return price;
			}
			set { 
				price = value;
			}
		}

		public double ScreenSize {
			get {
				return screenSize;
			}
			set { 
				screenSize = value;
			}
		}

		private Battery battery;

		public Battery Battery {
			get {
				return battery;
			}
			set { 
				battery = value;
			}
		}

		public Phone () {
			price = 0.0;
			screenSize = 0.0;
		}

		public Phone (String setModel, String setManufacturer, String setOwner, String setColor, double setPrice, double setScreenSize, Battery setBattery) {
			model = setModel;
			manufacturer = setManufacturer;
			owner = setOwner;
			color = setColor;

			price = setPrice;
			screenSize = setScreenSize;

			battery = setBattery;
		}

		public Phone (String setModel, String setManufacturer, String setColor, double setScreenSize, Battery setBattery) {
			model = setModel;
			manufacturer = setManufacturer;
			color = setColor;

			price = 0.0;
			screenSize = setScreenSize;

			battery = setBattery;
		}

		public override string ToString ()
		{
			return "Phone Information: \n\tModel: " + model + "\n\tManufacturer: " + manufacturer + "\n\tOwner: " + owner + "\n\tColor: " + color + "\n\tPrice : $" + price + "\n\tScreen Size: " + screenSize + "\"\n\tBattery: \n" + battery;
		}
	}
}
	