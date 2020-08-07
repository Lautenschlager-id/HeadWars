using System;
using System.Text.RegularExpressions;

namespace HeadWars
{
	// String functions
	static class Strings
	{
		// Variables
		private static Regex pattern;
		private static Match pfind;

		// Functions
		/// Repeats the string s in q times
		public static string repeatString(string s, int q)
		{
			string Out = s;
			for (int i = 1; i < q; i++)
				Out += s;
			return Out;
		}

		/// Matches a string
		public static Match match(string p, string v)
		{
			pattern = new Regex(p);
			pfind = pattern.Match(v);

			return pfind;
		}

		/// Returns a random generated char sequence
		public static string sequence(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
			char[] Out = new char[length];

			for (int i = 0; i < length; i++)
				Out[i] = chars[Maths.random.Next(chars.Length)];
			return new String(Out);
		}

		/// Creates a pattern replace
		public static string gsub(string str, string Pattern, string replacement)
		{
			return Regex.Replace(str, Pattern, replacement);
		}
	}
}