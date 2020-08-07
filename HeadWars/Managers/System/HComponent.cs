using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// Controls the components
	abstract class HComponent
	{
		// Variables
		public Boolean isWorking = true;
		public Boolean onClick = false;
		public float opacity = 1f;

		protected Boolean mouseHover = false;

		protected Color[] componentColor, contentColor;

		protected Texture2D componentTexture;
		protected Rectangle componentRectangle;

		protected Rectangle mouseRectangle;

		private SoundEffect hoverSound, clickSound;
		private Boolean useClicking;
		private Boolean wasHovering = false;
		private string name;
		private static List<string> names = new List<string>();

		// Properties
		public Boolean MouseHover { get { return mouseHover; } }
		public SoundEffect setHoverSound
		{
			set
			{
				hoverSound = value;
			}
		}
		public SoundEffect setClickSound
		{
			set
			{
				clickSound = value;
			}
		}
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value != "")
				{
					int i = 0;
					while (names.Contains(value))
						value = value.Substring(0, value.Length - i.ToString().Length) + (++i).ToString();
				}

				name = value;
			}
		}

		// Methods
		/// Constructor
		protected HComponent(int x, int y, int width, int height, Boolean execute = true, Boolean Clicking = false)
		{
			useClicking = Clicking;
			name = "";

			if (execute)
			{
				width = width <= 0 ? 1 : width;
				height = height <= 0 ? 1 : height;

				componentTexture = new Texture2D(HeadWars.Instance.GraphicsDevice, width, height);
				componentTexture.Fill(width * height);

				componentRectangle = new Rectangle(x, y, width, height);
			}
		}

		/// Change colors
		public virtual void setColor(Color[] component, Color[] content)
		{
			componentColor = component;
			contentColor = content;
		}
		public virtual void setColor(Color[] component)
		{
			componentColor = component;
		}

		/// Change position
		public virtual void setPosition(int x, int y)
		{
			componentRectangle.X = x;
			componentRectangle.Y = y;
		}
		/// Virtual because no need to be set in all the inheritances (as it should be using abstract)
		public virtual Vector2 setPosition(int x, int y, string align, float width, float height)
		{
			if (align.Contains("xcenter"))
				x += (int)(HeadWars.ScreenDimension.X / 2 - width / 2);
			else if (align.Contains("right"))
				x += (int)(HeadWars.ScreenDimension.X - width - 5);
			else if (align.Contains("left"))
				x += 5;

			if (align.Contains("ycenter"))
				y += (int)(HeadWars.ScreenDimension.Y / 2 - height / 2);
			else if (align.Contains("top"))
				y += 5;
			else if (align.Contains("bottom"))
				y += (int)(HeadWars.ScreenDimension.Y - height - 5);

			return new Vector2(x, y);
		}
		public virtual void setPosition() { }

		/// Update
		public virtual void Update()
		{
			mouseRectangle = new Rectangle((int)Control.MouseCoordinates.X, (int)Control.MouseCoordinates.Y, 1, 1);
			mouseHover = mouseRectangle.Intersects(componentRectangle);
			onClick = mouseHover ? (useClicking ? Control.MouseClicking : Control.MouseClicked) : false;

			if (isWorking)
			{
				if (onClick && clickSound != null)
					clickSound.Play(.3f, 0, 0);
				else if (mouseHover && !wasHovering && hoverSound != null)
					hoverSound.Play(.25f, 0, 0);
			}
			wasHovering = mouseHover;
		}

		/// Draw
		public virtual void Draw(SpriteBatch Layer1) { }
		public virtual void Draw(SpriteBatch Layer1, SpriteBatch Layer2) { }

	}
}