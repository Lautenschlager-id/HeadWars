using System;
using System.Collections.Generic;

namespace HeadWars
{
	class Info
	{
		// Variables
		public static int level = 1;
		public static int levelPoint = level - 1;

		/// Experience is given according to the score WITHOUT the multiplier
		public static int totalExperience = 6;

		// Methods
		/// Log in
		public static void Initialize()
		{
			DatabaseManager.Connect();
			foreach (SkillManager.Skill skill in SkillManager.skillData.Values)
			{
				List<object> stage = DatabaseManager.getPlayerData(HeadWars.playerName, "skillStage", "PlayerSkillH", "and skill='" + skill.Function + "'");

				if (stage.Count > 0)
				{
					for (int i = 0; i < (int)stage[0]; i++)
						typeof(SkillManager).GetMethod(skill.Function).Invoke(null, new object[] { });

					skill.CurrentStage = (int)stage[0];
				}
			}

			List<object> data = DatabaseManager.getPlayerData(HeadWars.playerName, "points, experience, highscore");
			DatabaseManager.Disconnect();

			level = (SkillManager.expToLevel(totalExperience = (int)data[1]))[0];

			levelPoint = (int)data[0];
			Score.highscore = (int)data[2];
		}

		// Log out
		public static void Conclude()
		{
			if (SkillManager.skillData["trickTreat"].isActive)
			{
				foreach (Enemy.Info e in Enemy.enemyInfo.Values)
					e.Reset();

				BlackHole.standLife /= 2;
				BlackHole.standScorePoint /= 2;
			}

			foreach (SkillManager.Skill skill in SkillManager.skillData.Values)
				skill.Reset();

			Score.Reset(!Player.Exists);
		}

		// Insert prize codes
		public static void NewPrizeCodes(int quantity)
		{
			DatabaseManager.Connect();
			for (int i = 0; i < quantity; i++)
				DatabaseManager.newPrizeCode(Strings.sequence(Maths.random.Next(10, 17)), Maths.random.Next(100, 900));
			DatabaseManager.Disconnect();
		}
	}
}
