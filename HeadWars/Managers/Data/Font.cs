using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HeadWars
{
	// Manages all the fonts
	static class Font
	{
		// Consts
		const int xBorder = 5;
		const int yBorder = 3;

		// Properties
		public static SpriteFont Game { get; private set; }
		public static SpriteFont Text { get; private set; }
		public static SpriteFont TextBold { get; private set; }
		public static SpriteFont BigTextBold { get; private set; }
		public static SpriteFont MediumTextBold { get; private set; }
		public static SpriteFont SmallTextBold { get; private set; }
		public static SpriteFont WindowTitle { get; private set; }

		// Methods
		/// Load
		public static void Load(ContentManager c)
		{
			Game = c.Load<SpriteFont>("Font/Game");
			Text = c.Load<SpriteFont>("Font/Text");
			TextBold = c.Load<SpriteFont>("Font/TextBold");
			BigTextBold = c.Load<SpriteFont>("Font/BigTextBold");
			MediumTextBold = c.Load<SpriteFont>("Font/MediumTextBold");
			SmallTextBold = c.Load<SpriteFont>("Font/SmallTextBold");
			WindowTitle = c.Load<SpriteFont>("Font/WindowTitle");
		}
	}
}