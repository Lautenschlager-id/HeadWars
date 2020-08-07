using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeadWars
{
	// The Black Hole entity
	class BlackHole : Entity
	{
		// Variables
		/// If a enemy has died with the black hole, it will explode
		public Boolean explode = false;
		public static int standLife = 50;
		public static int standScorePoint = 35;

		private int life = standLife;
		private int scorePoints = standScorePoint;
		private float existenceTime = 100f;
		private SkillManager.Skill attractionRange = SkillManager.skillData["bhEnemyRange"];
		private SkillManager.Skill playerAttractionRange = SkillManager.skillData["bhPlayerRange"];

		// Methods
		/// Constructor
		public BlackHole(Vector2 pos)
		{
			sprite = CharacterSelector.sprites[2][1];
			position = pos;
			radius = sprite.Width / 2f;

			angle = Maths.random.RandomRange(0, 360);
		}

		/// When the blackhole is fired
		public void onHit(Boolean destroy = false)
		{
			if (life-- <= 0 || destroy)
			{
				isDestroyed = true;

				if (!destroy)
				{
					Score.updateScore(scorePoints);
					Score.updateMultiplier();

					Sound.blackHoleExplosion.Play(.8f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
			}
		}

		/// Update
		public override void Update()
		{
			IEnumerable<Entity> entities = EntityManager.nearEntities(position, (float)attractionRange.Value, (float)playerAttractionRange.Value);

			foreach (Entity e in entities)
			{
				if (e is Enemy && (e as Enemy).isDestroyed)
					// Skips this loop
					continue;

				// Bullet = Repelled | Others = Attracted
				if (e is Bullet)
				{
					e.velocity += (e.position - position).scaleTo(.3f);

					// If the player is near, the sound is played
					if (Maths.Pythagoras(position, Player.Instance.position, 120))
						Sound.blackHoleRepelling.Play(.25f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
				else
				{
					Vector2 pos = position - e.position;
					float len = pos.Length();

					e.velocity += pos.scaleTo(MathHelper.Lerp(2, 0, len / (e is Player ? (float)playerAttractionRange.Value : (float)attractionRange.Value)));

					if (e is Player)
						Sound.blackHoleAttracting.Play(.15f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
			}

			if (explode)
			{
				existenceTime -= .1f;
				if (existenceTime <= 0)
				{
					EntityManager.New(new Explosion(Graphic.explosion, position, 80f));

					Sound.blackHoleExplosion.Play(.8f, Maths.random.RandomRange(-.2f, .2f), 0);

					isDestroyed = true;
				}
			}
		}

		/// Draw
		public override void Draw(SpriteBatch BackgroundLayer)
		{
			BackgroundLayer.Draw(sprite, position, null, color, angle, size / 2f, BackgroundLayer.Bounce(), 0, 0);
		}
	}
}