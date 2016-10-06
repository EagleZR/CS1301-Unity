using System;

namespace Lab_05
{
	public class Battery
	{
		private String model;
		public String Model {
			get {
				return model;
			}
			set {
				model = value;
			}
		}
			
		private double idleTime/*in seconds*/, hoursOfTalk; 

		public double IdleTime {
			get {
				return idleTime;
			}
			set {
				idleTime = value;
			}
		}

		public double HoursOfTalk {
			get {
				return hoursOfTalk;
			}
			set {
				hoursOfTalk = value;
			}
		}

		public Battery () {
			idleTime = 0;
			hoursOfTalk = 0;
		}

		public Battery (double setIdleTime, double setHoursOfTalk) {
			model = "";
			idleTime = setIdleTime;
			hoursOfTalk = setHoursOfTalk;
		}

		public Battery (String setModel, double setIdleTime, double setHoursOfTalk) {
			model = setModel;

		}

		public override string ToString ()
		{
			return "\t\tModel: " + model + "\n\t\tIdle Time: " + idleTime + "\n\t\tTalk Time: " + hoursOfTalk;
		}
	}
}

