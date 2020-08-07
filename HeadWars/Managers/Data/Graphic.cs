using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace HeadWars
{
	// Manages all the textures
	static class Graphic
	{
		// Properties
		/// Presentation images
		public static Texture2D[] presentation { get; private set; }
		#region Normal Mode
		/// Characters
		public static Texture2D player_red { get; private set; }
		public static Texture2D player_blue { get; private set; }
		public static Texture2D player_yellow { get; private set; }
		public static Texture2D player_green { get; private set; }
		public static Texture2D player_pink { get; private set; }

		public static Texture2D enemy_stalker { get; private set; }
		public static Texture2D enemy_wanderer { get; private set; }
		public static Texture2D enemy_wall { get; private set; }
		public static Texture2D enemy_bomber { get; private set; }
		public static Texture2D enemy_indestructible { get; private set; }
		public static Texture2D enemy_blocker { get; private set; }

		public static Texture2D blackhole { get; private set; }
		/// Objects
		public static Texture2D bullet { get; private set; }
		public static Texture2D asteroid { get; private set; }
		#endregion
		#region ETECH Mode
		/// Characters
		public static Texture2D teacher_furlan { get; private set; }
		public static Texture2D teacher_jota { get; private set; }
		public static Texture2D teacher_daniel { get; private set; }
		public static Texture2D teacher_akepeu { get; private set; }
		public static Texture2D teacher_mauricio { get; private set; }
		public static Texture2D teacher_virgilio { get; private set; }

		public static Texture2D jotahole { get; private set; }
		/// Objects
		public static Texture2D gradeMB { get; private set; }
		public static Texture2D gradeB { get; private set; }
		public static Texture2D gradeR { get; private set; }
		public static Texture2D gradeI { get; private set; }
		public static Texture2D paperAsteroid { get; private set; }
		#endregion
		/// Objects
		public static Texture2D explosion { get; private set; }
		/// Skills
		public static Texture2D[] skills { get; private set; }
		public static Texture2D lockedSkill { get; private set; }
		/// Languages
		public static Texture2D en { get; private set; }
		public static Texture2D pt { get; private set; }
		public static Texture2D es { get; private set; }
		public static Texture2D fr { get; private set; }
		/// Others
		public static Texture2D background { get; private set; }
		public static Texture2D mousePointer { get; private set; }
		public static Texture2D pause { get; private set; }
		public static Texture2D reverse { get; private set; }
		public static Texture2D no_code { get; private set; }

		// Methods
		/// Load
		public static void Load(ContentManager c)
		{
			presentation = Enumerable.Range(1, 3).Select(x => c.Load<Texture2D>("Graphic/Presentation/p" + x)).ToArray();

			player_red = c.Load<Texture2D>("Graphic/Object/Player/red");
			player_blue = c.Load<Texture2D>("Graphic/Object/Player/blue");
			player_yellow = c.Load<Texture2D>("Graphic/Object/Player/yellow");
			player_green = c.Load<Texture2D>("Graphic/Object/Player/green");
			player_pink = c.Load<Texture2D>("Graphic/Object/Player/pink");
			enemy_stalker = c.Load<Texture2D>("Graphic/Object/Enemy/stalker");
			enemy_wanderer = c.Load<Texture2D>("Graphic/Object/Enemy/wanderer");
			enemy_wall = c.Load<Texture2D>("Graphic/Object/Enemy/wall");
			enemy_bomber = c.Load<Texture2D>("Graphic/Object/Enemy/bomber");
			enemy_indestructible = c.Load<Texture2D>("Graphic/Object/Enemy/indestructible");
			enemy_blocker = c.Load<Texture2D>("Graphic/Object/Enemy/blocker");
			blackhole = c.Load<Texture2D>("Graphic/Object/Enemy/blackhole");

			teacher_furlan = c.Load<Texture2D>("Graphic/Object/Teacher/furlan");
			teacher_jota = c.Load<Texture2D>("Graphic/Object/Teacher/jota");
			teacher_daniel = c.Load<Texture2D>("Graphic/Object/Teacher/daniel");
			teacher_akepeu = c.Load<Texture2D>("Graphic/Object/Teacher/akepeu");
			teacher_mauricio = c.Load<Texture2D>("Graphic/Object/Teacher/mauricio");
			teacher_virgilio = c.Load<Texture2D>("Graphic/Object/Teacher/virgilio");
			jotahole = c.Load<Texture2D>("Graphic/Object/Teacher/jotahole");

			bullet = c.Load<Texture2D>("Graphic/Object/bullet");
			asteroid = c.Load<Texture2D>("Graphic/Object/asteroid");

			gradeMB = c.Load<Texture2D>("Graphic/Object/gradeMB");
			gradeB = c.Load<Texture2D>("Graphic/Object/gradeB");
			gradeR = c.Load<Texture2D>("Graphic/Object/gradeR");
			gradeI = c.Load<Texture2D>("Graphic/Object/gradeI");
			paperAsteroid = c.Load<Texture2D>("Graphic/Object/paperBall");

			explosion = c.Load<Texture2D>("Graphic/Object/explosion");

			skills = Enumerable.Range(1, 15).Select(x => c.Load<Texture2D>("Graphic/Skill/s" + (x - 1))).ToArray();
			lockedSkill = c.Load<Texture2D>("Graphic/Skill/sLocked");

			en = c.Load<Texture2D>("Graphic/Languages/en");
			pt = c.Load<Texture2D>("Graphic/Languages/pt");
			es = c.Load<Texture2D>("Graphic/Languages/es");
			fr = c.Load<Texture2D>("Graphic/Languages/fr");

			background = c.Load<Texture2D>("Graphic/background");
			mousePointer = c.Load<Texture2D>("Graphic/mousePointer");
			pause = c.Load<Texture2D>("Graphic/pause");
			reverse = c.Load<Texture2D>("Graphic/reverse");
			no_code = c.Load<Texture2D>("Graphic/nocode");
		}
	}
}