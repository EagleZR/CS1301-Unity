using System;

namespace lab_03_start
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			String sentence, result, another;
			do
			{
				Console.WriteLine ();
				Console.WriteLine ("Enter a sentence (no punctuation):");
				sentence = Console.ReadLine();
				Console.WriteLine ();
				result = translate (sentence);
				Console.WriteLine ("That sentence in Pig Latin is:");
				Console.WriteLine (result);
				Console.WriteLine ();
				Console.Write ("Translate another sentence (y/n)? ");
				another = Console.ReadLine();
			}
			while (another.Equals("y",StringComparison.InvariantCultureIgnoreCase));



		}
		//-----------------------------------------------------------------
		//  Divides the input string into individual words/tokens using 
		//  split, calls the translate method and when all the words have
		//  been translated, returns the entire translated line
		//-----------------------------------------------------------------
		public static string translate (string sentence)
		{
			string result = "";

				sentence = sentence.ToLower();  
				string [] words= sentence.Split(' ');
				for(int i=0; i<words.Length;i++)
			{
				result += translateWord (words[i]);
				result += " ";
			}
			return result;
		}
		//-----------------------------------------------------------------
		//  Translates one word into Pig Latin. If the word begins with a
		//  vowel, the suffix "yay" is appended to the word.  Otherwise,
		//  the first letter or two are moved to the end of the word,
		//  and "ay" is appended.
		//-----------------------------------------------------------------
		private static string translateWord (string word)
		{
			//test if the word begins with a vowel
			if (beginsWithVowel (word)) {
				word += "yay";
			}
			
			//else if the word begins with a blend
			else if (beginsWithBlend (word)) {
				String newWord = word.Substring (2);
				newWord += word.Substring (0, 2) + "ay";
				word = newWord;
			}
			
			//else the word begins with 1 or more than 2 consonants
			else {
				String newWord = word.Substring (1);
				newWord += word[0] + "ay";
				word = newWord;
			}
			
			return word;
		}
		//-----------------------------------------------------------------
		//  Determines if the specified word begins with a vowel.
		//-----------------------------------------------------------------
		private static bool beginsWithVowel (string word)
		{
			bool begins = false;
			begins = (word.StartsWith ("a") || word.StartsWith ("e") || word.StartsWith ("i") || word.StartsWith ("o") || word.StartsWith ("u"));
			return begins;
		}
		//-----------------------------------------------------------------
		//  Determines if the specified word begins with a particular
		//  two-character consonant blend.
		//-----------------------------------------------------------------
		private static bool beginsWithBlend (string word)
		{


			return (word.StartsWith ("bl") || word.StartsWith ("sc") ||
			        word.StartsWith ("br") || word.StartsWith ("sh") ||
			        word.StartsWith ("ch") || word.StartsWith ("sk") ||
			        word.StartsWith ("cl") || word.StartsWith ("sl") ||
			        word.StartsWith ("cr") || word.StartsWith ("sn") ||
			        word.StartsWith ("dr") || word.StartsWith ("sm") ||
			        word.StartsWith ("dw") || word.StartsWith ("sp") ||
			        word.StartsWith ("fl") || word.StartsWith ("sq") ||
			        word.StartsWith ("fr") || word.StartsWith ("st") ||
			        word.StartsWith ("gl") || word.StartsWith ("sw") ||
			        word.StartsWith ("gr") || word.StartsWith ("th") ||
			        word.StartsWith ("kl") || word.StartsWith ("tr") ||
			        word.StartsWith ("ph") || word.StartsWith ("tw") ||
			        word.StartsWith ("pl") || word.StartsWith ("wh") ||
			        word.StartsWith ("pr") || word.StartsWith ("wr") ); 
		}
	}
}
