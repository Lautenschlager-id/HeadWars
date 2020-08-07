using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeadWars
{
	// The character selector
	class CharacterSelector
	{
		// Variables
		public static List<List<Texture2D>> sprites = new List<List<Texture2D>>();

		List<HLabel> labels = new List<HLabel>(3);
		List<HButton> buttons = new List<HButton>(1);

		Texture2D characterPanelTexture;
		Rectangle characterPanelRectangle;
		Texture2D character;
		Vector2 characterPosition, characterSize;

		int currentCharacter = 0;

		List<List<List<string>>> spriteData = new List<List<List<string>>>();

		// Methods
		/// Constructor
		public CharacterSelector()
		{
			sprites.Clear();

			// Resizes the window
			HeadWars.Resize(570, 320);

			// Title
			labels.Add(new HLabel(Translation.GetString("character_selector"), Font.WindowTitle));
			labels[0].setPosition(0, 0, "xcenter top");

			// Next and Previous
			buttons.Add(new HButton(">>>>"));
			buttons[0].setPosition(110, 0, "xcenter ycenter");

			buttons.Add(new HButton("<<<<"));
			buttons[1].setPosition(-110, 0, "xcenter ycenter");

			// Select
			buttons.Add(new HButton(Translation.GetString("select")));
			buttons[2].setPosition(-5, -5, "right bottom");

			// Back to Menu
			buttons.Add(new HButton(Translation.GetString("button_back")));
			buttons[3].setPosition(5, -5, "left bottom");

			// Textures
			characterPanelTexture = new Texture2D(HeadWars.Instance.GraphicsDevice, 100, 100);
			characterPanelRectangle = new Rectangle((int)HeadWars.ScreenDimension.X / 2 - 50, (int)HeadWars.ScreenDimension.Y / 2 - 50, 100, 100);
			characterPosition = new Vector2(characterPanelRectangle.X + 50, characterPanelRectangle.Y + 50);

			characterPanelTexture.Border(20, Color.CadetBlue);

			// spriteData
			SetSprites();
			NormalizeCharacter();

			// Texts
			labels.Add(new HLabel("« " + GetSprite("name") + " »", Font.MediumTextBold));
			labels[1].setPosition(0, -80, "xcenter ycenter");
			labels[1].textColor(Color.Yellow);

			labels.Add(new HLabel(GetSprite("description"), Font.TextBold));
			labels[2].setPosition(0, 80, "xcenter ycenter");
		}

		/// Sets the Player / Enemy / Object lists
		private void SetSprites()
		{
			spriteData.Clear();
			if (HeadWars.mode == 0)
			{
				spriteData.Add(Sprite.normal_players);
				spriteData.Add(Sprite.normal_enemies);
				spriteData.Add(Sprite.normal_objects);
			}
			else if (HeadWars.mode == 1)
			{
				if (HeadWars.submode == 0)
				{
					spriteData.Add(Sprite.special_professors);
					spriteData.Add(Sprite.normal_enemies);
					spriteData.Add(Sprite.special_objects_MB);
				}
				else if (HeadWars.submode == 1)
				{
					spriteData.Add(Sprite.normal_players);
					spriteData.Add(Sprite.special_professors);
					spriteData.Add(Sprite.special_objects_I);
				}
			}
		}

		/// Get Player / Enemy data in the lists
		private string GetSprite(string field)
		{
			int i = 0;

			if (field == "name")
				i = 0;
			else if (field == "description")
				i = 1;
			else if (field == "graphic")
				i = 2;
			else if (field == "audio")
				i = 3;

			string Out = spriteData[0][currentCharacter][i];
			return (Out.Contains("_") && i < 2) ? Translation.GetString(Out) : Out;
		}

		/// Normalize character data
		private void NormalizeCharacter()
		{
			character = Sprite.GetGraphic(GetSprite("graphic"));
			characterSize = new Vector2(character.Width, character.Height);
			Sprite.GetSound(GetSprite("audio")).Play(.8f, 0, 0);
		}

		/// Update
		public void Update()
		{
			foreach (HButton b in buttons)
				b.Update();

			if (buttons[0].onClick || buttons[1].onClick)
			{
				if (buttons[0].onClick)
					currentCharacter++;
				else
					currentCharacter--;

				if (currentCharacter > spriteData[0].Count - 1)
					currentCharacter = 0;
				else if (currentCharacter < 0)
					currentCharacter = spriteData[0].Count - 1;

				labels[1].text = "« " + GetSprite("name") + " »";
				labels[2].text = GetSprite("description");

				labels[1].setPosition(0, -80, "xcenter ycenter");
				labels[2].setPosition(0, 80, "xcenter ycenter");

				NormalizeCharacter();
			}
			else if (buttons[2].onClick)
			{
				List<Texture2D> player = new List<Texture2D>() { Sprite.GetGraphic(GetSprite("graphic")) };
				List<Texture2D> enemies = new List<Texture2D>() { };
				List<Texture2D> objects = new List<Texture2D>() { };

				foreach (List<string> enemy in spriteData[1])
					enemies.Add(Sprite.GetGraphic(enemy[2]));
				foreach (List<string> obj in spriteData[2])
					objects.Add(Sprite.GetGraphic(obj[2]));

				sprites.Add(player);
				sprites.Add(enemies);
				sprites.Add(objects);

				HeadWars.currentGameState = HeadWars.gameState.Running;
				return;
			}
			else if (buttons[3].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.GameModes;
				return;
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer)
		{
			foreach (HButton b in buttons)
				b.Draw(BackgroundLayer, MediumLayer);
			foreach (HLabel l in labels)
				l.Draw(BackgroundLayer);

			BackgroundLayer.Draw(characterPanelTexture, characterPanelRectangle, Color.White);

			BackgroundLayer.Draw(character, characterPosition, null, Color.White, -MathHelper.PiOver2, characterSize / 2f, BackgroundLayer.Bounce(true, 1), 0, 0);
		}
	}
}