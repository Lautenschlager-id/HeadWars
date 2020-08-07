using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The account creation window
	class NewAccount
	{
		// Variables
		List<HTextBox> textboxes = new List<HTextBox>(2);
		List<HButton> buttons = new List<HButton>(2);
		List<HLabel> labels = new List<HLabel>(4);

		string warning;

		// Methods
		/// Constructor
		public NewAccount()
		{
			// Resizes the window
			HeadWars.Resize(310, 220);

			// Title
			labels.Add(new HLabel(Translation.GetString("new_account"), Font.WindowTitle));
			labels[0].setPosition(0, 0, "xcenter top");

			// Nickname
			labels.Add(new HLabel(Translation.GetString("username"), Font.TextBold, 30, 90));
			textboxes.Add(new HTextBox(30, 110, 12, false, 110));
			// Password
			labels.Add(new HLabel(Translation.GetString("password"), Font.TextBold, 30, 150));
			textboxes.Add(new HTextBox(30, 170, 14, true, 110));

			// Warning
			labels.Add(new HLabel(warning = "", Font.Text));
			labels[3].textColor(Color.Yellow);

			// Create account
			buttons.Add(new HButton(Translation.GetString("create_account"), 155, 110));
			// Back to Login
			buttons.Add(new HButton(Translation.GetString("button_back"), 155, 170));
		}

		/// Creates the account
		private Boolean createAccount(string nickname, string password)
		{
			Boolean Out = false;

			if (nickname.Length < 3)
				// Name : 3+
				warning = Translation.GetString("nickerror_charquantity");
			else if (!Char.IsLetter(nickname[0]))
				// Nickname must start with a letter
				warning = Translation.GetString("nickerror_firstchar");
			else if (password.Length < 5)
				// Pw : 5+
				warning = Translation.GetString("pwerror_charquantity");
			else
				Out = true;

			if (!Out)
				return Out;

			DatabaseManager.Connect();
			Out = DatabaseManager.CreatePlayer(nickname, password);
			if (!Out)
				warning = Translation.GetString("nickerror_exist");
			DatabaseManager.Disconnect();
			return Out;
		}

		/// Update
		public void Update()
		{
			foreach (HTextBox b in textboxes)
				b.Update();
			foreach (HButton b in buttons)
				b.Update();

			if (buttons[0].onClick || buttons[1].onClick)
			{
				Boolean go = true;
				if (buttons[0].onClick)
					go = createAccount(textboxes[0].answerText.ToLower(), textboxes[1].answerText);

				if (go)
					HeadWars.currentGameState = HeadWars.gameState.Login;
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
				labels[3].setPosition(0, 50, "xcenter top");
				labels[3].text = warning;
			}

			foreach (HLabel l in labels)
				l.Draw(BackgroundLayer);
		}
	}
}