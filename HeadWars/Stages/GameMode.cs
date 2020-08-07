using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeadWars
{
	// The game modes
	class GameMode
	{
		// Variables
		HLabel label;
		List<HButton> buttons = new List<HButton>(2);

		// Methods
		/// Constructor
		public GameMode()
		{
			HeadWars.mode = -1;

			buttons.Add(new HButton(Translation.GetString("normal")));
			buttons.Add(new HButton("ETECH"));
			// Back to Menu
			buttons.Add(new HButton(Translation.GetString("button_back")));

			// Resizes the window
			HeadWars.Resize(200, 70 + (buttons.Count * 40));

			for (int i = 0; i < buttons.Count; i++)
				buttons[i].setPosition(0, 70 + (i * 40), "xcenter");

			// Title
			label = new HLabel(Translation.GetString("modes"), Font.WindowTitle);
			label.setPosition(0, 0, "xcenter top");
		}

		/// Update
		public void Update()
		{
			foreach (HButton b in buttons)
				b.Update();

			if (buttons[0].onClick)
			{
				HeadWars.mode = 0;
				HeadWars.currentGameState = HeadWars.gameState.CharacterSelector;
				return;
			}
			else if (buttons[1].onClick)
			{
				HeadWars.mode = 1;
				HeadWars.currentGameState = HeadWars.gameState.ModeOption;
				return;
			}
			else if (buttons[2].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Menu;
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