using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeadWars
{
	// The Asteroid object
	class Asteroid : Entity
	{
		// Variables
		int life = 3;
		int subLife = 10;
		/// The size of each image since only one image is used and cropped "framely"
		int xWidth = 70, yHeight = 60;
		/// The source rectangle to fragment the sprites
		Rectangle rect;
		/// The origin since it's not the whole image
		Vector2 Size;
		// Player timer (do not destroy at once)
		float playerTimer = 0;

		// Methods
		/// Constructor
		public Asteroid(Vector2 pos)
		{
			sprite = CharacterSelector.sprites[2][2];
			position = pos;
			angle = Maths.random.RandomRange(0, 360);

			radius = (sprite.Width / 3) / 2f;

			rect = new Rectangle(life * xWidth, 0, xWidth, yHeight);
			Size = new Vector2(rect.Width, rect.Height);
		}

		/// When bullets or objects collides with the asteroid
		public void inCollision(Entity e)
		{
			if (e is Bullet)
				subLife--;
			else
				if (e is Player && playerTimer > 0 ? false : true)
				subLife -= 5;

			if (subLife <= 0)
			{
				subLife = 10;
				life--;
			}

			if (life <= 0)
				isDestroyed = true;
			else
			{
				Push(e, (e is Bullet ? 3 : 6));
				if (e is Player)
					playerTimer = 1f;
				Sound.skillAsteroid.Play(.6f, Maths.random.RandomRange(-.2f, .2f), 0);
			}
		}
		public void inCollision()
		{
			isDestroyed = true;
		}

		/// Update
		public override void Update()
		{
			rect.X = (life - 1) * xWidth;

			if (playerTimer > 0)
				playerTimer -= .3f;

			position += velocity;
			position = Vector2.Clamp(position, Size / 2f, HeadWars.ScreenDimension - Size / 2f);

			angle += velocity.Angle() / 300;
		}

		/// Draw
		public override void Draw(SpriteBatch BackgroundLayer)
		{
			if (!isDestroyed)
				BackgroundLayer.Draw(sprite, position, rect, color, angle, Size / 2f, 1f, 0, 0);
		}
	}
}