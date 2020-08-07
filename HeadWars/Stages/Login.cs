using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The login window
	class Login
	{
		// Variables
		List<HTextBox> textboxes = new List<HTextBox>(2);
		List<HButton> buttons = new List<HButton>(2);
		List<HLabel> labels = new List<HLabel>(4);

		string warning;

		// Methods
		/// Constructor
		public Login()
		{
			// Resizes the window
			HeadWars.Resize(310, 220);

			HeadWars.playerName = "";

			// Nickname
			labels.Add(new HLabel(Translation.GetString("username"), Font.TextBold, 30, 90));
			textboxes.Add(new HTextBox(30, 110, 12, false, 110));
			// Password
			labels.Add(new HLabel(Translation.GetString("password"), Font.TextBold, 30, 150));
			textboxes.Add(new HTextBox(30, 170, 14, true, 110));

			// Warning
			labels.Add(new HLabel(warning = "", Font.Text));
			labels[2].textColor(Color.Yellow);

			// Title
			labels.Add(new HLabel(Translation.GetString("login"), Font.WindowTitle));
			labels[3].setPosition(0, 0, "xcenter top");

			buttons.Add(new HButton(Translation.GetString("login"), 155, 110));
			buttons.Add(new HButton(Translation.GetString("new_account"), 155, 170));
		}

		/// Update
		public void Update()
		{
			foreach (HTextBox b in textboxes)
				b.Update();
			foreach (HButton b in buttons)
				b.Update();

			if (buttons[0].onClick)
			{
				// Name : 3+ | Pw : 5+
				if (textboxes[0].answerText.Length > 2 && textboxes[1].answerText.Length > 4)
				{
					Boolean userExists = false, canConnect = false;
					DatabaseManager.Connect();
					userExists = DatabaseManager.PlayerExists(textboxes[0].answerText.ToLower());
					if (userExists)
						canConnect = DatabaseManager.PlayerExists(textboxes[0].answerText.ToLower(), "and password='" + textboxes[1].answerText + "' collate nocase");
					DatabaseManager.Disconnect();

					if (canConnect)
					{
						HeadWars.playerName = textboxes[0].answerText.ToLower();
						Info.Initialize();
						HeadWars.currentGameState = HeadWars.gameState.Menu;
					}
					else
						warning = Translation.GetString(userExists ? "invalid_login" : "nickerror_notexist");
					return;
				}
				else
					warning = Translation.GetString("invalid_login");
			}
			else if (buttons[1].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.NewAccount;
				return;
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer)
		{
			foreach (HTextBox b in textboxes)
				b.Draw(BackgroundLayer, MediumLayer);
			foreach (HButton b in buttons)
				b.Draw(BackgroundLayer, MediumLayer);

			if (warning != "")
			{
				labels[2].text = warning;
				labels[2].setPosition(0, 50, "xcenter top");
			}

			foreach (HLabel l in labels)
				l.Draw(BackgroundLayer);
		}
	}
}