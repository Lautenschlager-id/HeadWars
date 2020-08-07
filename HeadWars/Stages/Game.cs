using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The game window
	public class Game
	{
		// Variables
		List<HLabel> labels = new List<HLabel>(4);
		List<HSquare> squares = new List<HSquare>(1);
		List<HButton> buttons = new List<HButton>(3);
		float gameOverTimer = 5f;

		Boolean isPaused = false;
		Boolean hasRecorded = false;

		// Methods
		/// Constructor
		public Game(Boolean skipMusic = false)
		{
			// Resizes the window
			HeadWars.Resize(850, 600);

			// Texts
			labels.Add(new HLabel("", Font.Game));
			labels.Add(new HLabel("", Font.Game));
			labels.Add(new HLabel("", Font.Game));
			labels.Add(new HLabel(string.Format(Translation.GetString("menu_redirect"), 0), Font.BigTextBold));
			labels[3].setPosition(0, 270, "xcenter");

			labels[1].textColor(Color.Red);
			labels[2].textColor(Color.Yellow);

			// Buttons
			// Pause
			squares.Add(new HSquare(Graphic.pause));
			squares[0].setColor(Color.Transparent.Collection());
			squares[0].setClickSound = Sound.toggleButton;

			// Continue
			buttons.Add(new HButton(Translation.GetString("new_game"), 0, 0, 120, 50));
			buttons[0].setPosition(0, 370, "xcenter");

			// Resume
			buttons.Add(new HButton(Translation.GetString("resume"), 0, 0, 200, 50));
			buttons[1].setFont(Font.BigTextBold);
			buttons[1].setPosition(0, -50, "xcenter ycenter");

			// Leave game
			buttons.Add(new HButton(Translation.GetString("leave_game"), 0, 0, 200, 50));
			buttons[2].setFont(Font.BigTextBold);
			buttons[2].setPosition(0, 50, "xcenter ycenter");

			// Menu
			buttons.Add(new HButton(Translation.GetString("menu"), 0, 0, 120, 50));
			buttons[3].setPosition(0, 440, "xcenter");

			EntityManager.Clear();
			Score.Reset();
			EntityManager.New(Player.Instance);

			if (!skipMusic)
			{
				MediaPlayer.Play(Sound.GameMusic);
				HeadWars.changedSoundtrack = true;
			}

			gameOverTimer = 5f;
			hasRecorded = false;
		}

		/// Saves highscore and xp
		private void Record()
		{
			SkillManager.updateLeveling();

			if (Score.beatedHighscore)
			{
				DatabaseManager.Connect();
				DatabaseManager.alterPlayerData(HeadWars.playerName, "highscore", Score.highscore);
				DatabaseManager.Disconnect();
			}
		}

		/// Update
		public void Update()
		{
			foreach (HSquare s in squares)
				s.Update();

			if (squares[0].onClick || Control.KeyUp(Keys.P))
				isPaused = !isPaused;

			if (isPaused)
			{
				buttons[1].Update();
				buttons[2].Update();

				if (buttons[1].onClick)
					isPaused = !isPaused;
				if (buttons[2].onClick)
				{
					Record();
					HeadWars.currentGameState = HeadWars.gameState.Menu;
				}
			}
			else
			{
				if (Score.gameOver)
				{
					// Updates only the explosions
					EntityManager.Update(true);

					buttons[0].Update();
					buttons[3].Update();

					if (buttons[0].onClick)
					{
						gameOverTimer = 5f;
						hasRecorded = false;
						new Game(true);
						return;
					}
					else if (buttons[3].onClick)
						gameOverTimer = 0;

					gameOverTimer -= isPaused ? 0 : .01f;
					if (gameOverTimer <= 0)
					{
						HeadWars.currentGameState = HeadWars.gameState.Menu;
						return;
					}
				}
				else
				{
					// Updates the entities
					EntityManager.Update();

					// Spawns enemies
					Spawner.Update();

					// Updates the score
					Score.Update();
				}
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer, SpriteBatch ForegroundLayer)
		{
			// Texts
			labels[0].text = string.Format("{0}: {1} x{2}", Translation.GetString("score"), Score.score, Score.scoreMultiplier);
			labels[0].setPosition(0, 0, "xcenter top");
			labels[0].textColor((Score.highscore > 0 && Score.beatedHighscore) ? Color.Yellow : Color.White);

			if (Score.gameOver)
			{
				if (!hasRecorded)
				{
					hasRecorded = true;
					Record();
				}

				/// Updates the final explosion
				EntityManager.Draw(ForegroundLayer, true);

				labels[2].text = string.Format("{0}\n{1}", Translation.GetString("game_over"), (Score.beatedHighscore ? string.Format("{0}!\n{1}", Translation.GetString("new_highscore"), Score.score) : string.Format("{0} : {1}\n{2} : {3}", Translation.GetString("highscore"), Score.highscore, Translation.GetString("score"), Score.score)));
				labels[2].setPosition(0, 130, "xcenter");

				labels[3].text = String.Format(Translation.GetString("menu_redirect"), Math.Ceiling(gameOverTimer));

				buttons[0].Draw(BackgroundLayer, MediumLayer);
				buttons[3].Draw(BackgroundLayer, MediumLayer);
			}
			else
			{
				// Sprites
				EntityManager.Draw(ForegroundLayer);

				labels[1].text = Strings.repeatString("♥", Score.life);
				labels[1].setPosition(0, 30, "xcenter top");
			}

			for (int i = 0; i < labels.Count; i++)
				if (i == 1 ? Score.life > 0 : (i > 1) ? Score.life <= 0 : true)
					labels[i].Draw(BackgroundLayer);

			foreach (HSquare s in squares)
				s.Draw(MediumLayer);

			if (isPaused)
			{
				buttons[1].Draw(ForegroundLayer);
				buttons[2].Draw(ForegroundLayer);
			}
		}
	}
}