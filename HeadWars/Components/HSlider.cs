using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HeadWars
{
	// A horizontal trackbar
	class HSlider : HComponent
	{
		// Variables
		/// All the content will be the line
		private Texture2D lineTexture;
		private Rectangle lineRectangle;

		private float valueScale;

		// Properties
		public float Value { get; private set; }
		public int xValue { get; private set; }

		// Methods
		/// Constructor
		public HSlider(int x, int y, int startAt = -1, int scale = 100, int width = 200, int height = 20)
			: base((startAt < 0 ? (x + (width / 2)) : startAt), y, 20, height, true, true)
		{
			valueScale = scale;

			lineTexture = new Texture2D(HeadWars.Instance.GraphicsDevice, width, 2);
			lineTexture.Fill(width * 2);

			lineRectangle = new Rectangle(x, y, width, 2);

			// Starts in the middle
			xValue = componentRectangle.X;
			checkPosition(true);

			componentColor = new Color[] { Color.Gray, Color.SlateGray };
			contentColor = Color.Gray.Collection();

			setHoverSound = null;
		}

		/// Gives the value according to the component X position
		private void checkPosition(Boolean setVal = false)
		{
			float xFinal = lineRectangle.X + (lineRectangle.Width - 10);

			float normalizer = 1 + ((lineRectangle.Width - 20) - 100) / valueScale;
			float val = setVal ? xValue : Maths.Clamp(Control.MouseCoordinates.X, lineRectangle.X + 10, xFinal);

			xValue = (int)val;

			Value = ((val - (lineRectangle.X + 10)) / valueScale) / normalizer;
			setPosition((int)val - 10, lineRectangle.Y);
		}

		/// Update
		public override void Update()
		{
			if (isWorking)
			{
				base.Update();

				if (onClick)
					checkPosition();
			}
		}

		/// Draw
		public override void Draw(SpriteBatch Layer)
		{
			if (isWorking)
			{
				Layer.Draw(lineTexture, lineRectangle, contentColor[0] * opacity);
				Layer.Draw(componentTexture, componentRectangle, componentColor[onClick ? 1 : 0] * opacity);
			}
		}
	}
}