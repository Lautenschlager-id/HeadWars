using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HeadWars
{
	// Manages all the entities and collisions at once
	static class EntityManager
	{
		// Variables
		public static float freezeEnemy = 0;
		public static Boolean unfreezing = false;

		/// Wheter the entities are being updated or not
		static Boolean updating;
		/// All the current entities
		static List<Entity> entities = new List<Entity>();
		/// All the new entities
		static List<Entity> newEntities = new List<Entity>();
		/// For the collision system, needs two lists to differs bullets of enemies as enemies collides with other enemies while bullets not
		static List<Bullet> bullets = new List<Bullet>();
		static List<Enemy> enemies = new List<Enemy>();
		static List<BlackHole> blackholes = new List<BlackHole>();
		static List<Explosion> explosions = new List<Explosion>();
		static List<Asteroid> asteroids = new List<Asteroid>();

		// Properties
		public static int Count
		{
			get
			{
				return entities.Count;
			}
		}
		public static int CountEnemy
		{
			get
			{
				return enemies.Count;
			}
		}
		public static int[] CountEnemyType
		{
			get
			{
				int[] Out = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
				foreach (Enemy e in enemies)
					Out[e.type]++;

				return Out;
			}
		}
		public static int CountBlackHole
		{
			get
			{
				return blackholes.Count;
			}
		}
		public static Boolean isFreeze
		{
			get
			{
				return freezeEnemy > 0;
			}
		}

		// Methods
		/// Checks if the given entity is a bullet or an enemy and adds it to the respective list
		private static void normalizeEntity(Entity e)
		{
			entities.Add(e);
			if (e is Bullet)
				bullets.Add(e as Bullet);
			else if (e is Enemy)
				enemies.Add(e as Enemy);
			else if (e is BlackHole)
				blackholes.Add(e as BlackHole);
			else if (e is Explosion)
				explosions.Add(e as Explosion);
			else if (e is Asteroid)
				asteroids.Add(e as Asteroid);
		}

		public static void New(Entity e)
		{
			// Checks wheter "e" is a new entity or an old one (that fits in the main list)
			if (!updating)
				normalizeEntity(e);
			else
				newEntities.Add(e);
		}

		public static void Clear()
		{
			entities.Clear();
			newEntities.Clear();
			bullets.Clear();
			enemies.Clear();
			blackholes.Clear();
			explosions.Clear();
			asteroids.Clear();
		}

		/// Get close entities
		public static IEnumerable<Entity> nearEntities(Vector2 pos, float r, float playerR)
		{
			return entities.Where(e => Maths.Pythagoras(pos, e.position, (e is Player ? playerR : r)));
		}

		/// Destroyes all the collisionable objects
		public static void resetEntities()
		{
			enemies.ForEach(e => e.onHit(true));
			blackholes.ForEach(b => b.onHit(true));
			asteroids.ForEach(a => a.inCollision());
		}

		/// Update
		public static void Update(Boolean onGameOver = false)
		{
			if (onGameOver)
			{
				if (explosions.Count > 0)
				{
					foreach (Explosion e in explosions)
						e.Update();
					explosions = explosions.Where(e => !e.isDestroyed).ToList();
				}
			}
			else
			{
				updating = true;

				CollisionHandler();

				// Updates all the entities in the main list
				foreach (Entity entity in entities)
					entity.Update();

				updating = false;

				// Add all the new entities (during the first looping) in the main list
				foreach (Entity entity in newEntities)
					normalizeEntity(entity);
				newEntities.Clear();

				// Removes all the destroyed entities from the main list (Technically, keeps all the non destroyed objects in the array)
				entities = entities.Where(e => !e.isDestroyed).ToList();
				bullets = bullets.Where(b => !b.isDestroyed).ToList();
				enemies = enemies.Where(e => !e.isDestroyed).ToList();
				blackholes = blackholes.Where(b => !b.isDestroyed).ToList();
				explosions = explosions.Where(e => !e.isDestroyed).ToList();
				asteroids = asteroids.Where(a => !a.isDestroyed).ToList();

				// Skills
				if (freezeEnemy > 0)
					freezeEnemy -= .008f;
				unfreezing = (freezeEnemy - .005f < .5f);
			}
		}

		/// Draw
		public static void Draw(SpriteBatch BackgroundLayer, Boolean onGameOver = false)
		{
			if (onGameOver)
				foreach (Explosion e in explosions)
					e.Draw(BackgroundLayer);
			else
				foreach (Entity entity in entities)
					entity.Draw(BackgroundLayer);
		}

		// Regions
		#region Collision

		/// Whether the entities are in collision or not
		private static Boolean inCollision(Entity e1, Entity e2)
		{
			float radius = e1.radius + e2.radius;

			// Both exist + Pythagoras circle detection
			return !e1.isDestroyed && !e2.isDestroyed && Maths.Pythagoras(e1.position, e2.position, radius);
		}

		/// Handler
		static void CollisionHandler()
		{
			/*
				Enemy x Enemy = Push
				Enemy x Bullet = A, B Die
				Enemy x BlackHole = A Die
				Enemy x Player = B Die
				Enemy x Explosion = A Die/Push
				Enemy x Asteroid = A, B Die
				BlackHole x Bullet = A Die, B Push
				BlackHole x Player = B Die
				BlackHole x Asteroid = B Die
				Explosion x Player = B Die
				Explosion x Asteroid = B Push
				Asteroid x Bullet = A, B Die
				Asteroid x Player = A Die
				Asteroid x Asteroid = A, B Push
			*/

			// Enemy x Enemy
			for (int i = 0; i < enemies.Count; i++)
				for (int j = i + 1; j < enemies.Count; j++)
					if (inCollision(enemies[i], enemies[j]))
					{
						enemies[i].Push(enemies[j]);
						enemies[j].Push(enemies[i]);
					}

			foreach (Enemy e in enemies)
				if (e.isWorking)
				{
					// Enemy x Bullet
					foreach (Bullet b in bullets)
						if (inCollision(e, b))
						{
							e.onHit();
							b.isDestroyed = true;
						}

					// Enemy x BlackHole
					foreach (BlackHole b in blackholes)
						if (inCollision(e, b))
						{
							e.onHit(true);
							b.explode = true;
						}

					// Enemy x Player
					if (!Player.Instance.isInvisible && inCollision(e, Player.Instance))
					{
						Player.Instance.Kill();
						// Break because everything was destroyed
						break;
					}

					// Enemy x Explosion (Closer = Kill, otherwise push)
					foreach (Explosion x in explosions)
					{
						if (Maths.Pythagoras(x.position, e.position, 45))
							if (Maths.Pythagoras(x.position, e.position, 25))
								e.onHit(true);
							else
								e.Push(x);
					}

					// Enemy x Asteroid
					foreach (Asteroid a in asteroids)
					{
						if (inCollision(e, a))
						{
							e.onHit(true);
							a.inCollision(e);
						}
					}
				}

			foreach (BlackHole b in blackholes)
			{
				// BlackHole x Bullet
				foreach (Bullet bu in bullets)
					if (inCollision(b, bu))
					{
						b.onHit();
						bu.isDestroyed = true;
					}

				// BlackHole x Player
				if (!Player.Instance.isInvisible && inCollision(b, Player.Instance))
				{
					Player.Instance.Kill();
					// Break because everything was destroyed
					break;
				}

				// BlackHole x Asteroid
				foreach (Asteroid a in asteroids)
					if (inCollision(b, a))
						a.inCollision(a);
			}

			foreach (Explosion e in explosions)
			{
				// Explosion x Player
				if (!Player.Instance.isInvisible && Maths.Pythagoras(e.position, Player.Instance.position, 45))
				{
					if (SkillManager.skillData["explosion"].isActive)
						Player.Instance.Push(e, 20);
					else
						Player.Instance.Kill();
					break;
				}

				// Explosion x Bullet
				foreach (Bullet b in bullets)
					if (inCollision(e, b))
						b.isDestroyed = true;

				// Explosion x Asteroid
				foreach (Asteroid a in asteroids)
					if (Maths.Pythagoras(e.position, a.position, 60))
						a.Push(e, 30);
			}

			foreach (Asteroid a in asteroids)
			{
				// Asteroid x Bullet
				foreach (Bullet b in bullets)
					if (inCollision(a, b))
					{
						a.inCollision(b);
						b.isDestroyed = true;
					}

				// Asteroid x Player
				if (!Player.Instance.isInvisible && inCollision(a, Player.Instance))
				{
					a.inCollision(Player.Instance);
					Player.Instance.Push(a);
				}
			}

			// Asteroid x Asteroid
			for (int i = 0; i < asteroids.Count; i++)
				for (int j = i + 1; j < asteroids.Count; j++)
					if (inCollision(asteroids[i], asteroids[j]))
					{
						asteroids[i].Push(asteroids[j]);
						asteroids[j].Push(asteroids[i]);
					}
		}

		#endregion
	}
}