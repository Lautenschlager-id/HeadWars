using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace HeadWars
{
	// The main menu
	class Menu
	{
		// Variables
		HLabel label;
		List<HButton> buttons = new List<HButton>(6);

		// Methods
		/// Constructor
		public Menu()
		{
			buttons.Add(new HButton(Translation.GetString("play")));
			buttons.Add(new HButton(Translation.GetString("skills")));
			buttons.Add(new HButton(Translation.GetString("options")));
			buttons.Add(new HButton(Translation.GetString("code")));
			buttons.Add(new HButton(Translation.GetString("credits")));
			buttons.Add(new HButton(Translation.GetString("logout")));

			// Resizes the window
			HeadWars.Resize(250, 130 + (buttons.Count * 32));

			for (int i = 0; i < buttons.Count; i++)
				buttons[i].setPosition(0, 60 + (i * 45), "xcenter");

			// Title
			label = new HLabel(Translation.GetString("menu"), Font.WindowTitle);
			label.setPosition(0, 0, "xcenter top");

			if (HeadWars.changedSoundtrack)
			{
				MediaPlayer.Play(Sound.MenuMusic);
				HeadWars.changedSoundtrack = false;
			}
		}

		/// Update
		public void Update()
		{
			foreach (HButton b in buttons)
				b.Update();

			if (buttons[0].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.GameModes;
				return;
			}
			else if (buttons[1].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.SkillTree;
				return;
			}
			else if (buttons[2].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Settings;
				return;
			}
			else if (buttons[3].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Code;
				return;
			}
			else if (buttons[4].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Credits;
				return;
			}
			else if (buttons[5].onClick)
			{
				Info.Conclude();
				HeadWars.currentGameState = HeadWars.gameState.Login;
				return;
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer)
		{
			foreach (HButton b in buttons)
				b.Draw(BackgroundLayer, MediumLayer);
			label.Draw(BackgroundLayer);
		}
	}
}