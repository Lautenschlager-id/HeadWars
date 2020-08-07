using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// Spawn enemies and other mortal objects
	class Spawner
	{
		// Classes
		public class Spawn
		{
			// Variables
			public int ID;
			public float Chance;
			public int Limit;
			public int Time;
			public string Name;
			public float SpawnTime;

			private float InitialChance;
			private float InitialSpawnTime;

			// Methods
			/// Constructor
			public Spawn(float chance, int limit, int time, string name, float nextSpawnTimer = 100f, Boolean setID = true)
			{
				if (setID)
					ID = enemyData.Count;

				Chance = chance;
				InitialChance = chance;
				Limit = limit;
				Time = time;
				Name = name;
				SpawnTime = InitialSpawnTime = nextSpawnTimer;
			}

			public void ResetChance()
			{
				Chance = InitialChance;
			}

			public void ResetSpawner()
			{
				SpawnTime = InitialSpawnTime;
			}
		}

		// Variables
		public static List<Spawn> enemyData = new List<Spawn>();
		public static Spawn blackHoleData;

		/// Range away from the spaceship
		private static float enemyRange = 200f;

		// Methods
		/// Constructor
		static Spawner()
		{
			// Name, Chance, Quantity Limit, Name, Elapsed time to spawnData
			enemyData.Add(new Spawn(110f, 15, 1, "Wanderer", 10f));
			enemyData.Add(new Spawn(180f, 8, 15, "Blocker"));
			enemyData.Add(new Spawn(320f, 6, 50, "Stalker", 200f));
			enemyData.Add(new Spawn(200f, 6, 100, "Wall"));
			enemyData.Add(new Spawn(250f, 6, 200, "Bomber", 80f));
			enemyData.Add(new Spawn(300f, 4, 300, "Indestructible"));

			blackHoleData = new Spawn(1500f, 2, 400, "BlackHole", 1000f, false);
		}

		/// Resets the chances when the players dies
		public static void restart()
		{
			foreach (Spawn s in enemyData)
				s.ResetChance();
			blackHoleData.ResetChance();
		}

		private static Vector2 spawnPoint()
		{
			Vector2 pos;

			do
			{
				pos = new Vector2(Maths.random.Next(10, (int)HeadWars.ScreenDimension.X - 10), Maths.random.Next(10, (int)HeadWars.ScreenDimension.Y - 10));
			} while (Maths.Pythagoras(pos, Player.Instance.position, enemyRange));

			return pos;
		}

		/// Update
		public static void Update()
		{
			// (150 entites max)
			if (!Player.Instance.isDead && EntityManager.Count < 151 && !EntityManager.isFreeze)
			{
				// Enemis
				int[] totalEnemies = EntityManager.CountEnemyType;
				foreach (Spawn s in enemyData)
				{
					// Must be after the given time + less than the limit quantity + chance
					if (--s.SpawnTime <= 0 && Player.Instance.currentTime >= s.Time && totalEnemies[s.ID] < s.Limit && Maths.random.Next(0, (int)s.Chance) == 0)
					{
						EntityManager.New((Entity)typeof(Enemy).GetMethod(s.Name).Invoke(null, new object[] { spawnPoint() }));
						if (s.Chance > 50)
							s.Chance -= .05f;
						s.ResetSpawner();
					}

					if (--blackHoleData.SpawnTime <= 0 && Player.Instance.currentTime >= blackHoleData.Time && EntityManager.CountBlackHole < blackHoleData.Limit && Maths.random.Next(0, (int)blackHoleData.Chance) == 0)
					{
						EntityManager.New(new BlackHole(spawnPoint()));
						if (blackHoleData.Chance > 500)
							blackHoleData.Chance -= .08f;
						blackHoleData.ResetSpawner();
					}
				}
			}
		}
	}
}