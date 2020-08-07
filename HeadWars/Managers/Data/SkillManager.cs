using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The skill handler
	class SkillManager
	{
		// Classes
		public class Skill
		{
			// Variables
			public Boolean isActive = false;
			public int ID;
			public string Name;
			public int Stages;
			public int CurrentStage = 0;
			public string Function;
			public string Description;

			private object Data;
			private int DataI = 0;
			private object FirstData;

			// Properties
			public object Value
			{
				get
				{
					return Data;
				}
				set
				{
					if (++DataI == 1)
						isActive = true;

					if (DataI <= Stages)
						Data = value;
				}
			}

			// Methods
			/// Constructor
			public Skill(int id, string name, object dataValue, int totalStages, string function, string description = "???")
			{
				ID = id;
				Name = name;
				Data = FirstData = dataValue;
				Stages = totalStages;
				Function = function;
				Description = description;
			}

			/// Reset
			public void Reset()
			{
				isActive = false;
				DataI = 0;
				CurrentStage = 0;
				Data = FirstData;
			}
		}

		// Consts
		/// 44 points are needed to complete the skill tree, though.
		const int maxLevel = 100;

		// Variables
		public static int expNextLevel = 0;

		public static Dictionary<string, Skill> skillData = new Dictionary<string, Skill>()
		{
			// Action name, value, stage. The id is decrescent. (0 = 15), (14 = 1)
			{"asteroid", new Skill(0, "skill_asteroid", 0, 3, "Asteroid", "skill_asteroid_description")},

			{"invisibility", new Skill(1, "skill_invisibility", 0f, 3, "Invisibility", "skill_invisibility_description")},
			{"freeze", new Skill(2, "skill_freeze", 0f, 3, "FreezeEnemy", "skill_freze_description")},

			{"shootSpeed", new Skill(3, "skill_shootspeed", 15, 5, "Shoot_Speed", "skill_shootspeed_description")},
			{"bullet", new Skill(4, "skill_bullet", 1, 2, "Bullets", "skill_bullet_description")},
			{"multiplier", new Skill(5, "skill_multiplier", 1, 3, "Multiplier", "skill_multiplier_description")},

			{"velocity", new Skill(6, "skill_velocity", 5, 5, "Velocity", "skill_velocity_description")},
			{"life", new Skill(7, "skill_life", 0, 1, "Extra_Life", "skill_life_description")},
			{"teleport", new Skill(8, "skill_teleport", false, 1, "Teleport", "skill_teleport_description")},
			{"bhEnemyRange", new Skill(9, "skill_bhenemyrange", 200f, 5, "BlackHole_EnemyRange", "skill_bhenemyrange_description")},

			{"explosion", new Skill(10, "skill_explosion", false, 1, "avoidExplosionKill", "skill_explosion_description")},
			{"trickTreat", new Skill(11, "skill_tricktreat", false, 1, "Trick_Treat", "skill_tricktreat_description")},
			{"extraScore", new Skill(12, "skill_extrascore", 0, 5, "Extra_Score", "skill_extrascore_description")},
			{"hitMultiplier", new Skill(13, "skill_hitmultiplier", false, 1, "hitMultiplier", "skill_hitmultiplier_description")},
			{"bhPlayerRange", new Skill(14, "skill_bhplayerrange", 200f, 5, "BlackHole_PlayerRange", "skill_bhplayerrange_description")},
		};

		// Methods
		/// Leveling
		public static int[] expToLevel(int exp)
		{
			int last = 0, total = 0, level = 0, need = 0, remain = 0;

			for (int i = 1; i <= maxLevel; i++)
			{
				int nLast = last + (i - 1) * (i < 16 ? 3 : 2) - (i < 6 ? -5 : (i < 11 ? -2 : 10));
				int nTotal = total + nLast;

				if (nTotal >= exp)
				{
					level = i - 1;
					need = nTotal - exp;
					remain = exp - total;
					break;
				}
				else
				{
					last = nLast;
					total = nTotal;
				}
			}

			return new int[] { level, need, remain };
		}

		public static int levelToExp(int level)
		{
			int last = 0, total = 0;
			for (int i = 1; i <= level; i++)
			{
				last += (i - 1) * (i < 16 ? 3 : 2) - (i < 6 ? -5 : (i < 11 ? -2 : 10));
				total += last;
			}

			return total;
		}

		public static void updateLeveling(Boolean ignore = false, Boolean save = true)
		{
			int[] newLevel = expToLevel(Info.totalExperience);

			if (newLevel[0] > Info.level || ignore)
			{
				Info.levelPoint += newLevel[0] - Info.level;
				Info.level = newLevel[0];

				expNextLevel = newLevel[1] + 1;
			}

			if (save)
			{
				DatabaseManager.Connect();
				DatabaseManager.alterPlayerData(HeadWars.playerName, "experience", Info.totalExperience);
				DatabaseManager.alterPlayerData(HeadWars.playerName, "points", Info.levelPoint);
				DatabaseManager.Disconnect();
			}
		}

		// Regions
		#region Skills
		/// Increases the player's velocity
		public static void Velocity()
		{
			if (skillData["velocity"].CurrentStage <= skillData["velocity"].Stages)
				skillData["velocity"].Value = (int)skillData["velocity"].Value + 2;
		}

		/// Increases the bullet quantity
		public static void Bullets()
		{
			if (skillData["bullet"].CurrentStage <= skillData["bullet"].Stages)
				skillData["bullet"].Value = (int)skillData["bullet"].Value + 1;
		}

		/// Invisibility for x seconds
		public static void Invisibility()
		{
			if (skillData["invisibility"].CurrentStage <= skillData["invisibility"].Stages)
				skillData["invisibility"].Value = (float)skillData["invisibility"].Value + 2f;
		}

		/// Blackhole stronger for enemies
		public static void BlackHole_EnemyRange()
		{
			if (skillData["bhEnemyRange"].CurrentStage <= skillData["bhEnemyRange"].Stages)
				skillData["bhEnemyRange"].Value = (float)skillData["bhEnemyRange"].Value + 10f;
		}

		/// BlackHole weaker for player
		public static void BlackHole_PlayerRange()
		{
			if (skillData["bhPlayerRange"].CurrentStage <= skillData["bhPlayerRange"].Stages)
				skillData["bhPlayerRange"].Value = (float)skillData["bhPlayerRange"].Value - 25f;
		}

		/// Increases the multiplier
		public static void Multiplier()
		{
			if (skillData["multiplier"].CurrentStage <= skillData["multiplier"].Stages)
				skillData["multiplier"].Value = (int)skillData["multiplier"].Value + 1;
		}

		/// Freezes the enemies for x seconds
		public static void FreezeEnemy()
		{
			if (skillData["freeze"].CurrentStage <= skillData["freeze"].Stages)
				skillData["freeze"].Value = (float)skillData["freeze"].Value + 2f;
		}

		/// Adds +1 initial life
		public static void Extra_Life()
		{
			if (skillData["life"].CurrentStage <= skillData["life"].Stages)
				skillData["life"].Value = (int)skillData["life"].Value + 1;
		}

		/// +30% for each enemy kill
		public static void Extra_Score()
		{
			if (skillData["extraScore"].CurrentStage <= skillData["extraScore"].Stages)
				skillData["extraScore"].Value = (int)skillData["extraScore"].Value + 6;
		}

		/// You doesn't loose the multiplier while you shoot the enemy
		public static void hitMultiplier()
		{
			if (skillData["hitMultiplier"].CurrentStage <= skillData["hitMultiplier"].Stages)
				skillData["hitMultiplier"].Value = true;
		}

		/// Player doesn't get killed by explosions, but pushed
		public static void avoidExplosionKill()
		{
			if (skillData["explosion"].CurrentStage <= skillData["explosion"].Stages)
				skillData["explosion"].Value = true;
		}

		/// Increases the shoot speed
		public static void Shoot_Speed()
		{
			if (skillData["shootSpeed"].CurrentStage <= skillData["shootSpeed"].Stages)
				skillData["shootSpeed"].Value = (int)skillData["shootSpeed"].Value - 2;
		}

		/// Enemy life and score are * 2
		public static void Trick_Treat()
		{
			if (skillData["trickTreat"].CurrentStage <= skillData["trickTreat"].Stages)
			{
				skillData["trickTreat"].Value = true;

				foreach (Enemy.Info e in Enemy.enemyInfo.Values)
				{
					e.Life *= 2;
					e.Score *= 2;
				}
				BlackHole.standLife *= 2;
				BlackHole.standScorePoint *= 2;
			}
		}

		/// Allows one teleport per round
		public static void Teleport()
		{
			if (skillData["teleport"].CurrentStage <= skillData["teleport"].Stages)
				skillData["teleport"].Value = true;
		}

		/// Player can shoot asteroids
		public static void Asteroid()
		{
			if (skillData["asteroid"].CurrentStage <= skillData["asteroid"].Stages)
				skillData["asteroid"].Value = (int)skillData["asteroid"].Value + 1;
		}

		#endregion
	}
}