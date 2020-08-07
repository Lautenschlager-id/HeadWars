using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The options
	public class Settings
	{
		// Variables
		private string[,] controls = new string[2, 4] { { "W", "A", "S", "D" }, { "←", "↑", "→", "↓" } };

		private List<HButton> buttons = new List<HButton>(6);
		private List<HSlider> sliders = new List<HSlider>(1);
		private List<HLabel> labels = new List<HLabel>(2);
		private List<HSquare> squares = new List<HSquare>(1);
		private List<HComboBox> comboboxes = new List<HComboBox>(1);

		// Slider X, Slider volume
		static object[] musicVolume = new object[] { -1, .5f };
		static object[] soundVolume = new object[] { -1, 1f };

		static Boolean inversed = false;

		// Methods
		/// Constructor
		public Settings()
		{
			// Resizes the window
			HeadWars.Resize(300, 400);

			// Title
			labels.Add(new HLabel(Translation.GetString("options"), Font.WindowTitle));
			labels[0].setPosition(0, 0, "xcenter top");

			// Volume slider
			labels.Add(new HLabel(Translation.GetString("music"), Font.TextBold, 25, 65));
			sliders.Add(new HSlider(30, 95, (int)musicVolume[0], 100, 100));

			labels.Add(new HLabel(Translation.GetString("sound"), Font.TextBold, 175, 65));
			sliders.Add(new HSlider(180, 95, (int)soundVolume[0], 100, 100));

			// Control
			labels.Add(new HLabel(Translation.GetString("movement"), Font.TextBold, 25, 125));
			labels.Add(new HLabel(Translation.GetString("aiming"), Font.TextBold, 185, 125));

			squares.Add(new HSquare(Graphic.reverse, 123, 120));
			squares[0].setColor(Color.Transparent.Collection());
			squares[0].setClickSound = Sound.toggleButton;

			for (int i = 0; i < 8; i++)
			{
				int id = (i > 3 ? i - 4 : i);
				HButton b = new HButton(controls[(i < 4 ? !inversed : inversed) ? 0 : 1, id], (i < 4 ? 25 : 185) + 21 * id, 155, 20, 20);
				b.setColor(Color.GhostWhite.Collection(), Color.Black.Collection());
				b.setFont(Font.TextBold);
				b.setClickSound = b.setHoverSound = null;
				buttons.Add(b);
			}

			// Fullscreen
			/*
			labels.Add(new HLabel(Translation.GetString("fullscreen"), Font.TextBold, 25, 190));
			buttons.Add(new HButton(HeadWars.fullscreen ? Translation.GetString("enabled") : Translation.GetString("disabled"), 25, 215));
			*/

			// Languages
			labels.Add(new HLabel(Translation.GetString("language"), Font.TextBold, 175, 190));
			comboboxes.Add(new HComboBox(Translation.langues, 175, 215, Translation.langues.FindIndex(l => l == Translation.CurrentLanguage)));

			// Back to Menu
			buttons.Add(new HButton(Translation.GetString("button_back"), 25, 360));
		}

		/// Update
		public void Update()
		{
			foreach (HSlider s in sliders)
				s.Update();
			foreach (HButton b in buttons)
				b.Update();
			foreach (HSquare s in squares)
				s.Update();
			foreach (HComboBox c in comboboxes)
				c.Update();

			if (sliders[0].Value != (float)musicVolume[1])
			{
				musicVolume[1] = sliders[0].Value;

				MediaPlayer.Volume = (float)musicVolume[1];

				musicVolume[0] = sliders[0].xValue;
			}
			if (sliders[1].Value != (float)soundVolume[1])
			{
				soundVolume[1] = sliders[1].Value;

				SoundEffect.MasterVolume = (float)soundVolume[1];

				soundVolume[0] = sliders[1].xValue;
			}

			/*if (buttons[8].onClick)
			{
				HeadWars.fullscreen = !HeadWars.fullscreen;
				buttons[8].text = HeadWars.fullscreen ? Translation.GetString("enabled") : Translation.GetString("disabled");
				labels[0].setPosition(0, 0, "xcenter top");
				new Settings();
			}
			else */if (buttons[8/*9*/].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Menu;
				return;
			}

			if (squares[0].onClick)
			{
				inversed = !inversed;

				for (int i = 0; i < 8; i++)
					buttons[i].text = controls[(i < 4 ? !inversed : inversed) ? 0 : 1, (i > 3 ? i - 4 : i)];

				Control.MoveToAim();
			}
			if (squares[0].MouseHover)
				squares[0].ContentAngle += .01f;
			else
				squares[0].ContentAngle = 0;

			if (comboboxes[0].ValueHasChanged)
			{
				Translation.setLanguage(comboboxes[0].Value);
				comboboxes[0].ValueHasChanged = false;
				HeadWars.options = new Settings();
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer, SpriteBatch ForegroundLayer)
		{
			foreach (HSlider s in sliders)
				s.Draw(BackgroundLayer);
			foreach (HButton b in buttons)
				b.Draw(BackgroundLayer, MediumLayer);
			foreach (HLabel l in labels)
				l.Draw(BackgroundLayer);
			foreach (HSquare s in squares)
				s.Draw(ForegroundLayer);
			foreach (HComboBox c in comboboxes)
				c.Draw(ForegroundLayer);
		}
	}
}