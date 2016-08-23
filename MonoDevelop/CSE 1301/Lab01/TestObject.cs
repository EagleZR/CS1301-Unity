using System;

namespace Lab01
{
	public class TestObject
	{
		private int number;
		private String words;

		public TestObject ()
		{
		}

		public TestObject (int number, string words){
			this.number = number;
			this.words = words;
		}
			
		public int getNumber(){
			return this.number;
		}

		public string getWords(){
			return this.words;
		}

	}
}

