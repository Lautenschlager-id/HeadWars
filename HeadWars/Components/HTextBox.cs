using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text.RegularExpressions;

namespace HeadWars
{
	// A textbox
	class HTextBox : HComponent
	{
		// Variables
		public string answerText = "";

		/// mouseHover acts like a selection here, but mouseHover is used to avoid memory waste
		private Keys[] keyboardKeys = new Keys[] {
			Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
			Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
			Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
			Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
			Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
			Keys.Z,

			Keys.NumPad0, Keys.NumPad1, Keys.NumPad2,
			Keys.NumPad3, Keys.NumPad4, Keys.NumPad5,
			Keys.NumPad6, Keys.NumPad7, Keys.NumPad8,
			Keys.NumPad9,

			Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4,
			Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,

			Keys.Back, Keys.OemMinus, Keys.Enter
		};

		private int maxCharacters;
		private Boolean hideText;
		private HLabel label;

		// Methods
		/// Constructor
		public HTextBox(int x, int y, int maxChar = 12, Boolean hide = false, int width = 100, int height = 30)
			: base(x, y, width, height)
		{
			maxCharacters = maxChar;
			hideText = hide;

			componentColor = new Color[] { Color.CadetBlue, Color.LightSlateGray };
			contentColor = new Color[] { Color.White, Color.WhiteSmoke };

			label = new HLabel(componentRectangle, "", Font.Text);
			label.setColor(contentColor);
		}

		/// Change position
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

		/// Change text font
		public void setFont(SpriteFont font)
		{
			label.font = font;
		}

		/// Key Handler
		private void handleKeyPressed(Keys key)
		{
			if (mouseHover)
			{
				if (key == Keys.Enter)
				{
					mouseHover = false;
					return;
				}
				else
				{
					if (key == Keys.Back)
					{
						if (answerText.Length != 0)
							answerText = answerText.Remove(answerText.Length - 1);
					}
					else
					{
						if (answerText.Length >= maxCharacters)
							return;

						if (key == Keys.OemMinus)
						{
							if (Control.Shift)
								answerText += "_";
							else
								answerText += "-";
						}
						else
						{
							// Matches numbers in the key name
							Match p = Strings.match(@"\d+", key.ToString());
							if (p.Success)
								answerText += p.Value;
							else
								answerText += Control.Shift ? key.ToString() : key.ToString().ToLower();
						}
					}

					label.text = hideText ? Strings.repeatString("*", answerText.Length) : answerText;
					label.setPosition();
				}
			}
		}

		/// Update
		public override void Update()
		{
			if (isWorking)
			{
				if (Control.MouseClicked)
				{
					mouseRectangle = new Rectangle((int)Control.MouseCoordinates.X, (int)Control.MouseCoordinates.Y, 1, 1);
					mouseHover = mouseRectangle.Intersects(componentRectangle);
				}

				if (mouseHover)
					foreach (Keys k in keyboardKeys)
						if (Control.KeyDown(k))
						{
							handleKeyPressed(k);
							break;
						}
			}
		}

		/// Draw
		public override void Draw(SpriteBatch Foreground)
		{
			Foreground.Draw(componentTexture, componentRectangle, componentColor[mouseHover ? 1 : 0] * opacity);
			if (answerText != "")
				label.Draw(Foreground);
		}
		public override void Draw(SpriteBatch Background, SpriteBatch MediumLayer)
		{
			if (isWorking)
			{
				Background.Draw(componentTexture, componentRectangle, componentColor[mouseHover ? 1 : 0] * opacity);
				if (answerText != "")
					label.Draw(MediumLayer);
			}
		}
	}
}