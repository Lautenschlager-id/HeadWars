using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeadWars
{
	// Class extesions
	static class Inheritances
	{
		// Functions
		/// Gets the angle between two points
		public static float Angle(this Vector2 v)
		{
			return (float)Math.Atan2(v.Y, v.X);
		}

		/// Transforms a Vector2 in a Point
		public static Point asPoint(this Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		/// Random.Next but with float
		public static float RandomRange(this Random r, float i, float j)
		{
			return (float)r.NextDouble() * (j - i) + i;
		}

		/// Returns the scalar value of the given value
		public static Vector2 scaleTo(this Vector2 v, float l)
		{
			return v * (l / v.Length());
		}

		/// Returns a new scale value
		public static float Bounce(this SpriteBatch s, Boolean preserve = true, int scale = 5)
		{
			return (preserve ? 1 : 0) + (float)Math.Sin(scale * HeadWars.GameTime.TotalGameTime.TotalSeconds) * (preserve ? .1f : 1);
		}

		/// Fills a rectangle texture
		public static void Fill(this Texture2D t, int size = 1)
		{
			Color[] fill = new Color[size];
			for (int i = 0; i < fill.Length; i++)
				fill[i] = Color.White;
			t.SetData(fill);
		}

		/// Fills the border of a rectangle texture
		public static void Border(this Texture2D t, int density, Color color)
		{
			Color[] fill = new Color[t.Width * t.Height];
			for (int x = 0; x < t.Width; x++)
			{
				for (int y = 0; y < t.Height; y++)
				{
					Boolean isColored = false;
					for (int i = 0; i <= density; i++)
					{
						if (x == i || y == i || x == t.Width - 1 - i || y == t.Height - 1 - i)
						{
							fill[x + y * t.Width] = color;
							isColored = true;
							break;
						}
					}

					if (!isColored)
						fill[x + y * t.Width] = Color.Transparent;
				}
			}
			t.SetData(fill);
		}

		/// Creates a color table with the same table
		public static Color[] Collection(this Color color, int indexes = 2)
		{
			Color[] Out = new Color[indexes];
			for (int i = 0; i < indexes; i++)
				Out[i] = color;
			return Out;
		}
	}
}