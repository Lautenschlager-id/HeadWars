using Microsoft.Xna.Framework;

namespace HeadWars
{
	// The bullet entity
	class Bullet : Entity
	{
		// Methods
		/// Constructor
		public Bullet(Vector2 pos, Vector2 vel)
		{
			sprite = CharacterSelector.sprites[2][0];

			position = pos;
			velocity = vel;
			angle = vel.Angle();

			color *= .8f;

			radius = 8;

			Sound.playerBullet.Play(.25f, Maths.random.RandomRange(-.2f, .2f), 0);
		}

		/// Update
		public override void Update()
		{
			if (velocity.LengthSquared() > 0)
				angle = velocity.Angle() + MathHelper.ToRadians(Maths.random.Next(-100, 100) / (HeadWars.mode == 1 ? 3 : 1));

			position += velocity;

			// Destroyes the entity when it's out of the screen
			if (!HeadWars.ViewPort.Bounds.Contains(position.asPoint()))
				isDestroyed = true;
		}
	}
}
