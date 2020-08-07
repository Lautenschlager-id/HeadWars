using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace HeadWars
{
	// Sound control
	static class Sound
	{
		// Variables
		/// Player
		private static SoundEffect[] player_bullet;
		private static SoundEffect[] skill_asteroid;
		private static SoundEffect[] teacherFurlan;
		private static SoundEffect[] teacherJota;
		private static SoundEffect[] teacherDaniel;
		private static SoundEffect[] teacherAkepeu;
		private static SoundEffect[] teacherMauricio;
		private static SoundEffect[] teacherVirgilio;

		// Properties
		/// Music
		public static Song GameMusic { get; private set; }
		public static Song MenuMusic { get; private set; }
		/// Enemy
		public static SoundEffect enemyExplosion { get; private set; }
		public static SoundEffect enemyDamage { get; private set; }
		/// Player
		public static SoundEffect playerExplosion { get; private set; }
		public static SoundEffect playerBullet
		{
			get
			{
				return player_bullet[Maths.random.Next(player_bullet.Length)];
			}
		}
		public static SoundEffect playerSpawn { get; private set; }
		public static SoundEffect playerLife { get; private set; }
		public static SoundEffect playerHighscore { get; private set; }
		/// Teachers
		public static SoundEffect teacher_furlan
		{
			get
			{
				return teacherFurlan[Maths.random.Next(teacherFurlan.Length)];
			}
		}
		public static SoundEffect teacher_jota
		{
			get
			{
				return teacherJota[Maths.random.Next(teacherJota.Length)];
			}
		}
		public static SoundEffect teacher_daniel
		{
			get
			{
				return teacherDaniel[Maths.random.Next(teacherDaniel.Length)];
			}
		}
		public static SoundEffect teacher_akepeu
		{
			get
			{
				return teacherAkepeu[Maths.random.Next(teacherAkepeu.Length)];
			}
		}
		public static SoundEffect teacher_mauricio
		{
			get
			{
				return teacherMauricio[Maths.random.Next(teacherMauricio.Length)];
			}
		}
		public static SoundEffect teacher_virgilio
		{
			get
			{
				return teacherVirgilio[Maths.random.Next(teacherVirgilio.Length)];
			}
		}
		/// Blackhole
		public static SoundEffect blackHoleExplosion { get; private set; }
		public static SoundEffect blackHoleAttracting { get; private set; }
		public static SoundEffect blackHoleRepelling { get; private set; }
		/// Components
		public static SoundEffect buttonHover { get; private set; }
		public static SoundEffect buttonClick { get; private set; }
		/// Skills
		public static SoundEffect skillTeleport { get; private set; }
		public static SoundEffect skillAsteroid
		{
			get
			{
				return skill_asteroid[Maths.random.Next(skill_asteroid.Length)];
			}
		}
		public static SoundEffect skillFreeze { get; private set; }
		public static SoundEffect skillInvisibility { get; private set; }
		/// Characters
		public static SoundEffect character_ship { get; private set; }
		/// Others
		public static SoundEffect skillHover { get; private set; }
		public static SoundEffect newSkill { get; private set; }
		public static SoundEffect redistributeSkills { get; private set; }
		public static SoundEffect toggleButton { get; private set; }

		// Methods
		/// Load
		public static void Load(ContentManager c)
		{
			GameMusic = c.Load<Song>("Sound/Soundtrack/Game");
			MenuMusic = c.Load<Song>("Sound/Soundtrack/Menu");

			enemyExplosion = c.Load<SoundEffect>("Sound/Game/Enemy/explosion");
			enemyDamage = c.Load<SoundEffect>("Sound/Game/Enemy/damage");
			playerExplosion = c.Load<SoundEffect>("Sound/Game/Player/explosion");
			player_bullet = Enumerable.Range(1, 2).Select(x => c.Load<SoundEffect>("Sound/Game/Player/bullet" + x)).ToArray();

			teacherFurlan = Enumerable.Range(1, 3).Select(x => c.Load<SoundEffect>("Sound/Character/Teacher/furlan" + x)).ToArray();
			teacherJota = Enumerable.Range(1, 2).Select(x => c.Load<SoundEffect>("Sound/Character/Teacher/jota" + x)).ToArray();
			teacherDaniel = Enumerable.Range(1, 3).Select(x => c.Load<SoundEffect>("Sound/Character/Teacher/daniel" + x)).ToArray();
			teacherAkepeu = Enumerable.Range(1, 6).Select(x => c.Load<SoundEffect>("Sound/Character/Teacher/akepeu" + x)).ToArray();
			teacherMauricio = Enumerable.Range(1, 2).Select(x => c.Load<SoundEffect>("Sound/Character/Teacher/mauricio" + x)).ToArray();
			teacherVirgilio = Enumerable.Range(1, 2).Select(x => c.Load<SoundEffect>("Sound/Character/Teacher/virgilio" + x)).ToArray();

			playerSpawn = c.Load<SoundEffect>("Sound/Game/Player/respawn");
			playerLife = c.Load<SoundEffect>("Sound/Game/Player/newLife");
			playerHighscore = c.Load<SoundEffect>("Sound/Game/Player/beatHighscore");
			blackHoleExplosion = c.Load<SoundEffect>("Sound/Game/Enemy/blackHoleExplosion");
			blackHoleAttracting = c.Load<SoundEffect>("Sound/Game/Enemy/blackHoleAttracting");
			blackHoleRepelling = c.Load<SoundEffect>("Sound/Game/Enemy/blackHoleRepelling");

			buttonHover = c.Load<SoundEffect>("Sound/Component/buttonHover");
			buttonClick = c.Load<SoundEffect>("Sound/Component/buttonClick");

			skillTeleport = c.Load<SoundEffect>("Sound/Game/Skill/teleport");
			skill_asteroid = Enumerable.Range(1, 3).Select(x => c.Load<SoundEffect>("Sound/Game/Skill/asteroid" + x)).ToArray();
			skillFreeze = c.Load<SoundEffect>("Sound/Game/Skill/freeze");
			skillInvisibility = c.Load<SoundEffect>("Sound/Game/Skill/invisibility");

			character_ship = c.Load<SoundEffect>("Sound/Character/ship");

			skillHover = c.Load<SoundEffect>("Sound/skillHover");
			newSkill = c.Load<SoundEffect>("Sound/newSkill");
			redistributeSkills = c.Load<SoundEffect>("Sound/redistributeSkills");
			toggleButton = c.Load<SoundEffect>("Sound/toggleButton");
		}
	}
}