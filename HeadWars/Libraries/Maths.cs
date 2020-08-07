using System;
using Microsoft.Xna.Framework;

namespace HeadWars
{
	// Math functions
	static class Maths
	{
		// Variables
		public static Random random = new Random();

		// Functions
		/// Polarity to Vectorization
		public static Vector2 polarToVector(float angle, float magnitude)
		{
			return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		/// Pythagoras (in circle detection)
		public static Boolean Pythagoras(Vector2 p1, Vector2 p2, float range)
		{
			return Vector2.DistanceSquared(p1, p2) < (range * range);
		}

		/// Sets a range for the given value
		public static float Clamp(float val, float min, float max)
		{
			return Math.Max(min, Math.Min(max, val));
		}

		/// X% of T
		public static float Percent(int x, int t)
		{
			return (x / 100) * t;
		}
	}
}