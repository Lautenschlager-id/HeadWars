using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeadWars
{
	// Credits during the game, not the initial ones
	class Credits
	{
		// Variables
		static List<List<object>> creditsData = new List<List<object>> {
			// ID, Name, Description
			new List<object> {0, "Tainã Romani Lautenschlager Donda", "credit_developer"},
			new List<object> {1, "Felipe Gonçalves Gomes", "credit_graphic"},
		};

		List<HLabel> labels = new List<HLabel>(1 + creditsData.Count);
		List<HLabel> sublabels = new List<HLabel>(creditsData.Count);
		List<HSquare> squares = new List<HSquare>(creditsData.Count);
		List<HButton> buttons = new List<HButton>(1);

		// Methods
		/// Constructor
		public Credits()
		{
			// Resizes the window
			HeadWars.Resize(350, 150 + ((creditsData.Count - 1) * 40));

			// Title
			labels.Add(new HLabel(Translation.GetString("credits"), Font.WindowTitle));
			labels[0].setPosition(0, 0, "xcenter top");

			foreach (List<object> o in creditsData)
			{
				labels.Add(new HLabel((string)o[1], Font.TextBold, 20, 75 + ((int)o[0] * 40)));
				sublabels.Add(new HLabel(Translation.GetString((string)o[2]), Font.Text));
				squares.Add(new HSquare(0, 0, (int)sublabels[(int)o[0]].measureString().X, (int)sublabels[(int)o[0]].measureString().Y));
			}

			// Back to Menu
			buttons.Add(new HButton(Translation.GetString("button_back")));
			buttons[0].setPosition(0, 0, "left bottom");
		}

		/// Update
		public void Update()
		{
			foreach (HButton b in buttons)
				b.Update();
			foreach (HLabel l in labels)
				l.Update();

			if (buttons[0].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Menu;
				return;
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer, SpriteBatch ForegroundLayer)
		{
			foreach (HButton b in buttons)
				b.Draw(BackgroundLayer, MediumLayer);
			foreach (HLabel l in labels)
				l.Draw(BackgroundLayer);

			foreach (List<object> o in creditsData)
				if (labels[(int)o[0] + 1].MouseHover)
				{
					// When the mouse is on a label, it opens the popup
					squares[(int)o[0]].setPosition((int)Control.MouseCoordinates.X + 15, (int)Control.MouseCoordinates.Y - 15);
					squares[(int)o[0]].Draw(ForegroundLayer);

					sublabels[(int)o[0]].setPosition((int)Control.MouseCoordinates.X + 15, (int)Control.MouseCoordinates.Y - 15);
					sublabels[(int)o[0]].Draw(ForegroundLayer);
				}
		}
	}
}