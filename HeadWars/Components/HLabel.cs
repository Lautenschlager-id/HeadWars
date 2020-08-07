using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HeadWars
{
	// A Label
	class HLabel : HComponent
	{
		// Variables
		private Vector2 textPosition;
		private Boolean hasRectangle = false;
		private Rectangle sourceRectangle;
		private string Text = "";
		private Boolean toggleColor;
		private SpriteFont textFont;

		// Properties
		public string text
		{
			get
			{
				return Text != null ? Text : "";
			}
			set
			{
				Text = value;
				if (hasRectangle)
					setPosition();
			}
		}
		public SpriteFont font
		{
			get
			{
				return textFont;
			}
			set
			{
				textFont = value;
			}
		}
		public Color TextColor { get; private set; }

		// Methods
		/// Constructor
		public HLabel(string labelText, SpriteFont textFont, int x = 10, int y = 10, Boolean hoverEffect = false)
			: base(0, 0, 0, 0, false)
		{
			text = labelText;

			textPosition = new Vector2(x, y);

			contentColor = Color.White.Collection();
			toggleColor = hoverEffect;

			font = textFont;

			TextColor = Color.White;
		}
		/// Overloads the constructor method for rectangle aligned labels
		public HLabel(Rectangle sourceRect, string labelText, SpriteFont textFont)
			: base(0, 0, 0, 0, false)
		{
			text = labelText;

			componentColor = Color.White.Collection();
			toggleColor = false;

			font = textFont;

			hasRectangle = true;
			sourceRectangle = sourceRect;
			setPosition();
		}

		/// Get text size
		public Vector2 measureString()
		{
			return font.MeasureString(text);
		}

		/// Update rectangle
		public void updateSourceRectangle(Rectangle sourceRect)
		{
			if (hasRectangle)
				sourceRectangle = sourceRect;
		}

		/// Change position
		public override void setPosition(int x, int y)
		{
			textPosition = new Vector2(x, y);
			if (hasRectangle)
				base.setPosition(x, y);
		}
		public override Vector2 setPosition(int x, int y, string align, float width = 1f, float height = 1f)
		{
			Vector2 offset = base.setPosition(x, y, align, measureString().X, measureString().Y);
			setPosition((int)offset.X, (int)offset.Y);

			return offset;
		}
		public override void setPosition()
		{
			Vector2 rCorner = new Vector2(sourceRectangle.Left, sourceRectangle.Top);
			Vector2 rDimension = new Vector2(sourceRectangle.Width, sourceRectangle.Height);

			Vector2 sSize = measureString();

			Vector2 offset = (rDimension - sSize) / 2;

			textPosition = rCorner + offset;
		}

		/// Text color
		public void textColor(Color color)
		{
			TextColor = color;
			contentColor = color.Collection();
		}

		/// Update
		public override void Update()
		{
			if (isWorking)
			{
				mouseRectangle = new Rectangle((int)Control.MouseCoordinates.X, (int)Control.MouseCoordinates.Y, 1, 1);
				mouseHover = mouseRectangle.Intersects(new Rectangle((int)textPosition.X, (int)textPosition.Y, (int)measureString().X, (int)measureString().Y));
			}
		}

		/// Draw
		public override void Draw(SpriteBatch Layer)
		{
			if (isWorking)
			{
				Layer.DrawString(font, text, textPosition, (hasRectangle ? componentColor[(toggleColor && mouseHover) ? 1 : 0] : contentColor[(toggleColor && mouseHover) ? 1 : 0]) * opacity);
			}
		}
	}
}