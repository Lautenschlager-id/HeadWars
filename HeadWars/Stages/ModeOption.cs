using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeadWars
{
	// The options of the choosen mode (if exists)
	class ModeOption
	{
		// Variables
		HLabel label;
		List<HButton> buttons = new List<HButton>(3);

		// Methods
		/// Constructor
		public ModeOption()
		{
			if (HeadWars.mode == 1)
			{
				buttons.Add(new HButton("MB"));
				buttons.Add(new HButton("I"));
			}

			// Back to GameMode
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

			if (buttons[2].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.GameModes;
				return;
			}
			else if (buttons[0].onClick || buttons[1].onClick)
			{
				HeadWars.submode = buttons[0].onClick ? 0 : 1;
				HeadWars.currentGameState = HeadWars.gameState.CharacterSelector;
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