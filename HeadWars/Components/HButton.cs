using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeadWars
{
	// A button
	class HButton : HComponent
	{
		// Variables
		private HLabel label;

		// Properties
		public string text
		{
			get
			{
				return label.text;
			}
			set
			{
				label.text = value;
			}
		}

		// Methods
		/// Constructor
		public HButton(string buttonText = "", int x = 10, int y = 10, int width = 120, int height = 30)
			: base(x, y, width, height)
		{
			componentColor = new Color[] { Color.CadetBlue, Color.LightSlateGray };
			contentColor = new Color[] { Color.White, Color.WhiteSmoke };

			label = new HLabel(componentRectangle, buttonText, Font.Text);
			label.setColor(contentColor);

			setHoverSound = Sound.buttonHover;
			setClickSound = Sound.buttonClick;
		}

		/// Set position
		public override void setPosition(int x, int y)
		{
			base.setPosition(x, y);

			label.updateSourceRectangle(componentRectangle);
			label.setPosition();
		}
		public override Vector2 setPosition(int x, int y, string align, float width = 1f, float height = 1f)
		{
			Vector2 offset = base.setPosition(x, y, align, componentRectangle.Width, componentRectangle.Height);

			setPosition((int)offset.X, (int)offset.Y);

			return offset;
		}

		/// Set color
		public override void setColor(Color[] component, Color[] content)
		{
			base.setColor(component, content);
			label.setColor(content);
		}

		/// Set text font
		public void setFont(SpriteFont font)
		{
			label.font = font;
		}

		/// Update
		public override void Update()
		{
			if (isWorking)
				base.Update();
		}

		/// Draw
		public override void Draw(SpriteBatch ForegroundLayer)
		{
			if (isWorking)
			{
				ForegroundLayer.Draw(componentTexture, componentRectangle, componentColor[mouseHover ? 1 : 0] * opacity);
				label.Draw(ForegroundLayer);
			}
		}
		public override void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer)
		{
			if (isWorking)
			{
				BackgroundLayer.Draw(componentTexture, componentRectangle, componentColor[mouseHover ? 1 : 0] * opacity);
				label.Draw(MediumLayer);
			}
		}
	}
}