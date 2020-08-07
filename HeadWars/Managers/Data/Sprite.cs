using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeadWars
{
	// Manages the sprites according to the given mode
	class Sprite
	{
		// Variables
		public static List<List<string>> normal_players = new List<List<string>>()
		{
			// Name, Description, Graphic, Audio
			new List<string> {"player_red_name", "playerdescription_red", "player_red", "character_ship"},
			new List<string> {"player_blue_name", "playerdescription_blue", "player_blue", "character_ship"},
			new List<string> {"player_yellow_name", "playerdescription_yellow", "player_yellow", "character_ship"},
			new List<string> {"player_green_name", "playerdescription_green", "player_green", "character_ship"},
			new List<string> {"player_pink_name", "playerdescription_pink", "player_pink", "character_ship"},
		};
		public static List<List<string>> normal_enemies = new List<List<string>>()
		{
			// Name, Description, Graphic
			new List<string> {"Wanderer", "no_description", "enemy_wanderer"},
			new List<string> {"Blocker", "no_description", "enemy_blocker"},
			new List<string> {"Stalker", "no_description", "enemy_stalker"},
			new List<string> {"Wall", "no_description", "enemy_wall"},
			new List<string> {"Bomber", "no_description", "enemy_bomber"},
			new List<string> {"Indestructible", "no_description", "enemy_indestructible"},
		};
		public static List<List<string>> special_professors = new List<List<string>>()
		{
			// Name, Description, Graphic, Audio
			new List<string> {"Men", "teacherdescription_mauricio", "teacher_mauricio", "teacher_mauricio"},
			new List<string> {"Giota", "teacherdescription_jota", "teacher_jota", "teacher_jota"},
			new List<string> {"Furtado", "teacherdescription_furlan", "teacher_furlan", "teacher_furlan"},
			new List<string> {"Super Sarrajin", "teacherdescription_virgilio", "teacher_virgilio", "teacher_virgilio"},
			new List<string> {"Pilu de Flango", "teacherdescription_daniel", "teacher_daniel", "teacher_daniel"},
			new List<string> {"Pew Pew", "teacherdescription_akepeu", "teacher_akepeu", "teacher_akepeu"},
		};
		public static List<List<string>> special_objects_MB = new List<List<string>>()
		{
			// Name, Description, Graphic
			new List<string> {"Bullet", "no_description", "gradeMB"},
			new List<string> {"Black Hole", "no_description", "blackhole"},
			new List<string> {"Asteroid", "no_description", "paperAsteroid"},
		};
		public static List<List<string>> normal_objects = new List<List<string>>()
		{
			// Name, Description, Graphic
			new List<string> {"Good Grade", "no_description", "bullet"},
			new List<string> {"Black Hole", "no_description", "blackhole"},
			new List<string> {"Paper Ball", "no_description", "asteroid"},
		};
		public static List<List<string>> special_objects_I = new List<List<string>>()
		{
			// Name, Description, Graphic
			new List<string> {"Bad Grade", "no_description", "gradeI"},
			new List<string> {"Jota Hole", "no_description", "jotahole"},
			new List<string> {"Paper Ball", "no_description", "paperAsteroid"},
		};

		// Methods
		/// Gets the Texture2D
		public static Texture2D GetGraphic(string name)
		{
			return (Texture2D)typeof(Graphic).GetProperty(name, typeof(Texture2D)).GetValue(null, new object[] { });
		}
		/// Gets the Audio
		public static SoundEffect GetSound(string name)
		{
			return (SoundEffect)typeof(Sound).GetProperty(name, typeof(SoundEffect)).GetValue(null, new object[] { });
		}
	}
}