using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HeadWars
{
	// A panel component
	class HSquare : HComponent
	{
		// Variables
		public float ContentAngle = 0;

		Boolean toggleColor;
		Boolean hasContent = false;
		Texture2D contentTexture;
		Vector2 contentPosition, contentSize;

		// Properties
		public Texture2D GetContent { get { return hasContent ? contentTexture : null; } }

		// Methods
		/// Constructor
		public HSquare(int x = 10, int y = 10, int width = 50, int height = 50, Boolean toggleOnMouseHover = false)
			: base(x, y, width, height)
		{
			componentColor = new Color[] { Color.CadetBlue, Color.WhiteSmoke };
			toggleColor = toggleOnMouseHover;
		}
		/// Constructor override for content
		public HSquare(Texture2D sourceImage, int x = 10, int y = 10, int width = 50, int height = 50)
			: this(x, y, width, height)
		{
			hasContent = true;

			contentTexture = sourceImage;

			contentPosition = new Vector2(x + (width / 2), y + (height / 2));

			contentSize = new Vector2(sourceImage.Width, sourceImage.Height);
		}

		/// Change content texture
		public void setContent(Texture2D sourceImage)
		{
			if (hasContent)
				contentTexture = sourceImage;
		}

		/// Change position
		public override void setPosition(int x, int y)
		{
			if (hasContent)
				contentPosition = new Vector2(x + (componentRectangle.Width / 2), y + (componentRectangle.Height / 2));
			base.setPosition(x, y);
		}

		/// Update
		public override void Update()
		{
			if (isWorking)
				base.Update();
		}

		/// Draw
		public override void Draw(SpriteBatch Layer)
		{
			if (isWorking)
			{
				Layer.Draw(componentTexture, componentRectangle, componentColor[(toggleColor && mouseHover) ? 1 : 0] * opacity);
				if (hasContent)
					Layer.Draw(contentTexture, contentPosition, null, Color.White * opacity, ContentAngle, contentSize / 2f, 1f, 0, 0);
			}
		}
	}
}