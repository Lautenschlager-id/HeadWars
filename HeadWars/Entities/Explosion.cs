using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HeadWars
{
	// The Explosion Effect
	class Explosion : Entity
	{
		// Variables
		float timer = 0, interval;
		int currentFrame = 1;
		/// The size of each image since only one image is used and cropped "framely"
		int xWidth = 96, yHeight = 96;
		/// The source rectangle to fragment the sprites
		Rectangle rect;
		/// The origin since it's not the whole image
		Vector2 Size;

		// Methods
		/// Constructor
		public Explosion(Texture2D explosionSprite, Vector2 pos, float intervalTime = 50f, Boolean sound = false)
		{
			sprite = explosionSprite;
			position = pos;

			radius = xWidth / 2f;

			interval = intervalTime;

			if (sound)
				Sound.enemyExplosion.Play(.5f, Maths.random.RandomRange(-.2f, .2f), 0);
		}

		/// Update
		public override void Update()
		{
			angle = Maths.random.RandomRange(0, 360);
			timer += (float)HeadWars.GameTime.ElapsedGameTime.TotalMilliseconds;

			// Next frame
			if (timer > interval)
			{
				if (++currentFrame == 20)
					isDestroyed = true;

				timer = 0f;
			}

			rect = new Rectangle(currentFrame * xWidth, 0, xWidth, yHeight);
			Size = new Vector2(rect.Width, rect.Height);
		}

		/// Draw
		public override void Draw(SpriteBatch BackgroundLayer)
		{
			if (!isDestroyed)
				BackgroundLayer.Draw(sprite, position, rect, color, angle, Size / 2f, 1f, 0, 0);
		}
	}
}