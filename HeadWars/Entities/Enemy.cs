using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HeadWars
{
	// All the enemy settings and types
	class Enemy : Entity
	{
		// Classes
		public class Info
		{
			// Variables
			public int ID;
			public int Life, FirstLife;
			public int Score, FirstScore;

			// Methods
			/// Constructor
			public Info(int id, int life, int score)
			{
				ID = id;
				Life = FirstLife = life;
				Score = FirstScore = score;
			}

			/// Reset
			public void Reset()
			{
				Life = FirstLife;
				Score = FirstScore;
			}
		}

		// Variables
		public static Dictionary<string, Info> enemyInfo = new Dictionary<string, Info>() {
			// Type (id), Life, Score Points
			{"Wanderer", new Info(0, 1, 2)},
			{"Blocker", new Info(1, 5, 3)},
			{"Stalker", new Info(2, 6, 5)},
			{"Wall", new Info(3, 5, 4)},
			{"Bomber", new Info(4, 15, 10)},
			{"Indestructible", new Info(5, 20, 15)}
		};

		private List<IEnumerator<int>> actions = new List<IEnumerator<int>>();
		/// Timer when the enemy is spawning
		private int loadingFrame = 20;
		private int scorePoints;
		private int damageColor = 0;
		private Boolean isOnHit = false;

		// Properties
		public int type { get; private set; }
		public int life { get; private set; }
		public Boolean isWorking
		{
			get
			{
				return loadingFrame <= 0;
			}
		}

		// Methods
		/// Constructor
		public Enemy(Vector2 pos, int enemyType, int enemyLife, int addScore)
		{
			sprite = CharacterSelector.sprites[1][enemyType];
			position = pos;
			radius = sprite.Width / 2f;
			color = Color.Transparent;

			type = enemyType;
			life = enemyLife;
			scorePoints = addScore;
		}

		/// When the enemy is fired
		public void onHit(Boolean destroy = false)
		{
			if (--life <= 0 || destroy)
			{
				isDestroyed = true;

				if (!destroy)
				{
					int points = scorePoints;
					if (SkillManager.skillData["extraScore"].isActive)
						points += (int)Maths.Percent((int)SkillManager.skillData["extraScore"].Value, points);

					Score.updateScore(points);
					Score.updateMultiplier();

					Sound.enemyExplosion.Play(.4f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
			}
			else
			{
				// Gets slower for a while
				velocity *= .4f;

				if (SkillManager.skillData["hitMultiplier"].isActive)
					Score.normalizeTimeLeft();

				// Makes the enemy red-transparent
				color = Color.LightPink * .85f;
				// Timer to go back to white
				damageColor = 10;

				Sound.enemyDamage.Play(.35f, Maths.random.RandomRange(-.2f, .2f), 0);

				if (!isOnHit)
					isOnHit = true;
			}
		}

		/// Sets the action for the enemy
		private void setAction(IEnumerable<int> a)
		{
			actions.Add(a.GetEnumerator());
		}

		/// Resume the coroutines
		private void executeActions()
		{
			for (int i = 0; i < actions.Count; i++)
			{
				if (!actions[i].MoveNext())
					// i-- to avoid pointer exception
					actions.RemoveAt(i--);
			}
		}

		/// Update
		public override void Update()
		{
			if (isWorking)
			{
				if (!EntityManager.isFreeze)
					executeActions();

				if (damageColor > 0)
				{
					damageColor--;
					if (damageColor <= 0)
						color = Color.White;
				}
			}
			else
			{
				loadingFrame--;

				// Alpha system [0 ~> 1]
				color = Color.White * (1 - loadingFrame / 60f);
			}

			if (EntityManager.isFreeze)
				color = EntityManager.unfreezing ? Color.White : Color.CadetBlue * .95f;
			else
			{
				position += velocity;
				position = Vector2.Clamp(position, size / 2, HeadWars.ScreenDimension - size / 2);

				// Friction-like effect
				velocity *= .8f;
			}
		}

		// Regions
		#region Enemy Types

		/// Stalks the player
		IEnumerable<int> Stalk(float speedForce = .7f)
		{
			while (true)
			{
				// Constant rate
				velocity += (Player.Instance.position - position).scaleTo(speedForce);
				if (velocity != Vector2.Zero)
					angle = velocity.Angle();
				yield return 0;
			}
		}

		/// Moves randomly
		IEnumerable<int> RandMove()
		{
			// Won't be const as it will change periodically
			// 0 - 360 in rad
			float dir = Maths.random.RandomRange(0, MathHelper.TwoPi);

			Rectangle border = HeadWars.ViewPort.Bounds;
			border.Inflate(-sprite.Width, -sprite.Height);

			while (true)
			{
				dir += Maths.random.RandomRange(-.5f, .5f);
				// MathHelper.WrapAngle normalizes the angle in a range [-π : π]
				dir = MathHelper.WrapAngle(dir);

				for (int i = 0; i < 6; i++)
				{
					velocity += Maths.polarToVector(dir, .4f);
					angle -= .05f;

					if (!border.Contains(position.asPoint()))
						dir = (HeadWars.ScreenDimension / 2 - position).Angle() + Maths.random.RandomRange(-MathHelper.PiOver2, MathHelper.PiOver2);

					yield return 0;
				}
			}
		}

		/// Moves horizontaly or vertically
		IEnumerable<int> LineLoop(float speed = 3)
		{
			Boolean changeY = Maths.random.Next(0, 2) == 0;

			Rectangle border = HeadWars.ViewPort.Bounds;
			border.Inflate(-sprite.Width / 2 - 1, -sprite.Height / 2 - 1);

			while (true)
			{
				if (!border.Contains(position.asPoint()))
					speed = -speed;

				if (changeY)
					velocity = Vector2.UnitY * speed;
				else
					velocity = Vector2.UnitX * speed;
				angle = velocity.Angle() + MathHelper.PiOver2;

				yield return 0;
			}
		}

		/// Explodes after some time, while it alters its function (stalker or wanderer)
		IEnumerable<int> Bomb()
		{
			float currentTime = 0;
			int act = 1;

			while (true)
			{
				currentTime += .01f;

				if ((int)Math.Floor(currentTime) % 5 == 0)
				{
					if (actions.Count > 1)
						actions.RemoveAt(1);

					if (act++ % 2 == 0)
						setAction(Stalk(.4f));
					else
						setAction(RandMove());
					currentTime++;
				}


				if (currentTime > 12)
				{
					EntityManager.New(new Explosion(Graphic.explosion, position, 40f, true));

					isDestroyed = true;
				}

				yield return 0;
			}
		}

		/// Moves after some time, works like a block
		IEnumerable<int> Block()
		{
			while (true)
			{
				if (isOnHit)
				{
					velocity += new Vector2(Maths.random.Next(-10, 10), Maths.random.Next(-10, 10));
					angle = velocity.Angle();

					isOnHit = false;
				}

				yield return 0;
			}
		}

		/// Doesn't loose life when it's in an orange-like color
		IEnumerable<int> Shield(int constLife = 8)
		{
			float currentTime = 0;

			while (true)
			{
				currentTime += .008f;

				if (currentTime <= 6)
				{
					color = Color.Chartreuse;
					life = constLife;
				}
				else if (currentTime == 6.008)
					color = Color.White;
				else if (currentTime > 10)
					currentTime = 0;

				yield return 0;
			}
		}

		#endregion

		#region Enemies

		public static Enemy Stalker(Vector2 pos)
		{
			Enemy e = new Enemy(pos, enemyInfo["Stalker"].ID, enemyInfo["Stalker"].Life, enemyInfo["Stalker"].Score);
			e.setAction(e.Stalk());

			return e;
		}

		public static Enemy Wanderer(Vector2 pos)
		{
			Enemy e = new Enemy(pos, enemyInfo["Wanderer"].ID, enemyInfo["Wanderer"].Life, enemyInfo["Wanderer"].Score);
			e.setAction(e.RandMove());

			return e;
		}

		public static Enemy Wall(Vector2 pos)
		{
			Enemy e = new Enemy(pos, enemyInfo["Wall"].ID, enemyInfo["Wall"].Life, enemyInfo["Wall"].Score);
			e.setAction(e.LineLoop());

			return e;
		}

		public static Enemy Bomber(Vector2 pos)
		{
			Enemy e = new Enemy(pos, enemyInfo["Bomber"].ID, enemyInfo["Bomber"].Life, enemyInfo["Bomber"].Score);
			e.setAction(e.Bomb());

			return e;
		}

		public static Enemy Blocker(Vector2 pos)
		{
			Enemy e = new Enemy(pos, enemyInfo["Blocker"].ID, enemyInfo["Blocker"].Life, enemyInfo["Blocker"].Score);
			e.setAction(e.Block());

			return e;
		}

		public static Enemy Indestructible(Vector2 pos)
		{
			Enemy e = new Enemy(pos, enemyInfo["Indestructible"].ID, enemyInfo["Indestructible"].Life, enemyInfo["Indestructible"].Score);
			e.setAction(e.RandMove());
			e.setAction(e.Shield());

			return e;
		}

		#endregion
	}
}