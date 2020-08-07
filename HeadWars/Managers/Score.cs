using System;

namespace HeadWars
{
	// Score and life manager
	class Score
	{
		// Consts
		const float multiplierMaxTime = 1f;
		const int maxMultiplier = 20;
		const int initialLifeScore = 800;

		// Variables
		public static SkillManager.Skill multiplierIncrease = SkillManager.skillData["multiplier"];
		public static int highscore = 0;
		public static int maxLife = 5;
		public static Boolean beatedHighscore = false;

		/// Timer to destroy the multiplier
		private static float multiplierTimeLeft;
		private static int newLifeScore = initialLifeScore;
		private static int givenLives = 0;
		private static Boolean isMaxLife = false;

		// Properties
		public static int score { get; private set; }
		public static int scoreMultiplier { get; private set; }
		public static int life { get; set; }
		public static Boolean gameOver
		{
			get
			{
				return life <= 0;
			}
		}

		// Methods
		/// Constructor
		static Score()
		{
			Reset(true);
		}

		public static void Reset(Boolean first = false)
		{
			if (beatedHighscore)
				highscore = score;

			score = 0;

			beatedHighscore = false;

			scoreMultiplier = 1;
			multiplierTimeLeft = 0;

			newLifeScore = initialLifeScore;
			givenLives = 0;

			life = 2 + (int)SkillManager.skillData["life"].Value;

			if (!first)
				Player.Instance.Reset();
		}

		/// Damage
		public static void Damage()
		{
			life--;
			isMaxLife = false;
		}

		/// Scoring
		public static void updateMultiplier()
		{
			if (Player.Instance.isDead)
				return;

			multiplierTimeLeft = multiplierMaxTime;
			if (scoreMultiplier < maxMultiplier)
				scoreMultiplier += (int)multiplierIncrease.Value;
		}

		public static void updateScore(int addScore)
		{
			if (Player.Instance.isDead)
				return;

			score += addScore * scoreMultiplier;

			if (score > highscore && !beatedHighscore)
			{
				beatedHighscore = true;
				Sound.playerHighscore.Play(.8f, Maths.random.RandomRange(-.2f, .2f), 0);
			}

			if (score >= newLifeScore)
			{
				newLifeScore += (givenLives += 2) * initialLifeScore;

				if (life <= maxLife)
				{
					life++;
					if (!isMaxLife)
					{
						if (life >= maxLife)
							isMaxLife = true;
						Sound.playerLife.Play(1f, Maths.random.RandomRange(-.2f, .2f), 0);
					}

					life = (int)Maths.Clamp(life, 0, maxLife);
				}
			}

			Info.totalExperience += addScore;
			SkillManager.updateLeveling(false, false);
		}

		/// For skill purposes
		public static void normalizeTimeLeft()
		{
			multiplierTimeLeft = multiplierMaxTime;
		}

		/// Update
		public static void Update()
		{
			if (scoreMultiplier > 1)
				if ((multiplierTimeLeft -= (float)HeadWars.GameTime.ElapsedGameTime.TotalSeconds) <= 0)
				{
					multiplierTimeLeft = multiplierMaxTime;
					scoreMultiplier = 1;
				}
		}
	}
}
