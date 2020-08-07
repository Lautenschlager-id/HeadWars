using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace HeadWars
{
	// Main class
	public class HeadWars : Microsoft.Xna.Framework.Game
	{
		// Enums		
		public enum gameState
		{
			Presentation,

			Login,
			NewAccount,

			Menu,
			GameModes,
			// Etech has two options, so needs a class
			ModeOption,
			// Choose the player
			CharacterSelector,
			SkillTree,
			Settings,
			Code,
			Credits,

			Running,
			Paused,
		}

		// Variables
		static Boolean Fullscreen = false;

		static GraphicsDeviceManager graphics;
		SpriteBatch BackgroundLayer;
		SpriteBatch MediumLayer;
		SpriteBatch ForegroundLayer;
		/// Menus
		static Presentation presentation;
		static Login login;
		static NewAccount newAccount;
		static Menu mainMenu;
		static Game game;
		static GameMode gameMode;
		static ModeOption modeOption;
		static CharacterSelector characterSelector;
		static SkillTree skillTree;
		public static Settings options;
		static Code code;
		static Credits credits;

		static gameState CurrentGameState = gameState.Presentation;
		public static Boolean changedSoundtrack = false;

		// Properties
		public static HeadWars Instance { get; private set; }
		/// Screen dimensions with the properties below
		public static Viewport ViewPort
		{
			get
			{
				return Instance.GraphicsDevice.Viewport;
			}
		}
		public static Vector2 ScreenDimension
		{
			get
			{
				return new Vector2(ViewPort.Width, ViewPort.Height);
			}
		}
		public static GameTime GameTime { get; private set; }
		/// Changes the state and updates the buttons
		public static gameState currentGameState
		{
			get { return CurrentGameState; }
			set
			{
				if (CurrentGameState != value)
				{
					CurrentGameState = value;

					switch (CurrentGameState)
					{
						case gameState.Login:
							login = new Login();
							break;
						case gameState.NewAccount:
							newAccount = new NewAccount();
							break;
						case gameState.Menu:
							mainMenu = new Menu();
							break;
						case gameState.GameModes:
							gameMode = new GameMode();
							break;
						case gameState.ModeOption:
							modeOption = new ModeOption();
							break;
						case gameState.CharacterSelector:
							characterSelector = new CharacterSelector();
							break;
						case gameState.SkillTree:
							skillTree = new SkillTree();
							break;
						case gameState.Settings:
							options = new Settings();
							break;
						case gameState.Code:
							code = new Code();
							break;
						case gameState.Credits:
							credits = new Credits();
							break;
						case gameState.Running:
							game = new Game();
							break;
					}
				}
			}
		}
		/// Fullscreen
		public static Boolean fullscreen
		{
			get
			{
				return Fullscreen;
			}
			set
			{
				Fullscreen = value;
				graphics.ToggleFullScreen();
			}
		}
		/// Player's nickname
		public static string playerName { get; set; }
		/// Game mode (Normal [0], Etech [1])
		public static int mode { get; set; }
		public static int submode { get; set; }
		public static Color background { get; private set; }

		// Methods
		/// Constructor
		public HeadWars()
		{
			graphics = new GraphicsDeviceManager(this);

			background = Color.Black;

			// XNA Standard
			Content.RootDirectory = "Content";

			Window.Title = "Head Wars";

			Instance = this;
		}

		/// Resizes the window in runtime
		public static void Resize(int w, int h)
		{
			if (!fullscreen)
			{
				// Size will never be bigger than the screen size of the computer
				if (w > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
					w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				if (h > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
					h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

				graphics.PreferredBackBufferWidth = w;
				graphics.PreferredBackBufferHeight = h;
				graphics.ApplyChanges();
			}
		}

		/// Initialize anything non-graphic before running the application
		protected override void Initialize()
		{
			// XNA Standard
			base.Initialize();

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = .5f;
			SoundEffect.MasterVolume = 1f;

			// Language
			Translation.setLanguage(Translation.WindowsLanguage);

			// Loads the database at least once to avoid delays
			DatabaseManager.Connect();
			DatabaseManager.Disconnect();
		}

		/// Loads all the contents once per game (after Initialize())
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			BackgroundLayer = new SpriteBatch(GraphicsDevice);
			MediumLayer = new SpriteBatch(GraphicsDevice);
			ForegroundLayer = new SpriteBatch(GraphicsDevice);

			BackgroundLayer.Name = "Background";
			MediumLayer.Name = "Medium";
			ForegroundLayer.Name = "Foreground";

			// Loads all the art
			Graphic.Load(Content);

			// Loads all the fonts
			Font.Load(Content);

			// Loads all the sounds
			Sound.Load(Content);

			// Creates the menus
			presentation = new Presentation();
		}

		/// For non-oollectable-garbage. Runs once per game
		protected override void UnloadContent() { }

		/// The loop (Default: 60x per frame)
		protected override void Update(GameTime gameTime)
		{
			if (!IsActive) return;

			GameTime = gameTime;

			// Control (Keyboard, Mouse)
			Control.Update();

			// Allows to exit the game
			if (Control.KeyDown(Keys.Escape))
				Exit();

			switch (currentGameState)
			{
				case gameState.Presentation:
					presentation.Update();
					break;
				case gameState.Login:
					login.Update();
					break;
				case gameState.NewAccount:
					newAccount.Update();
					break;
				case gameState.Menu:
					mainMenu.Update();
					break;
				case gameState.GameModes:
					gameMode.Update();
					break;
				case gameState.ModeOption:
					modeOption.Update();
					break;
				case gameState.CharacterSelector:
					characterSelector.Update();
					break;
				case gameState.SkillTree:
					skillTree.Update();
					break;
				case gameState.Settings:
					options.Update();
					break;
				case gameState.Code:
					code.Update();
					break;
				case gameState.Credits:
					credits.Update();
					break;
				case gameState.Running:
					game.Update();
					break;
			}

			// XNA Standard
			base.Update(gameTime);
		}

		/// Called to draw each frame
		float backgroundPositionX = -50, backgroundPositionY = -20;
		int backgroundXVelocity = 1, backgroundYVelocity = 1;
		protected override void Draw(GameTime gameTime)
		{
			if (!IsActive) return;

			// Background
			GraphicsDevice.Clear(background);

			backgroundPositionX += .05f * backgroundXVelocity;
			if (backgroundPositionX < -400 || backgroundPositionX > -50)
				backgroundXVelocity = -backgroundXVelocity;

			backgroundPositionY += .01f * backgroundYVelocity;
			if (backgroundPositionY < -100 || backgroundPositionY > -1)
				backgroundYVelocity = -backgroundYVelocity;

			BackgroundLayer.Begin();
			BackgroundLayer.Draw(Graphic.background, new Vector2(backgroundPositionX, backgroundPositionY), Color.White);
			BackgroundLayer.End();

			ForegroundLayer.Begin();
			MediumLayer.Begin();
			BackgroundLayer.Begin();

			switch (currentGameState)
			{
				case gameState.Presentation:
					presentation.Draw(BackgroundLayer);
					break;
				case gameState.Login:
					login.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.NewAccount:
					newAccount.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.Menu:
					mainMenu.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.GameModes:
					gameMode.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.ModeOption:
					modeOption.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.CharacterSelector:
					characterSelector.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.SkillTree:
					skillTree.Draw(BackgroundLayer, MediumLayer, ForegroundLayer);
					break;
				case gameState.Settings:
					options.Draw(BackgroundLayer, MediumLayer, ForegroundLayer);
					break;
				case gameState.Code:
					code.Draw(BackgroundLayer, MediumLayer);
					break;
				case gameState.Credits:
					credits.Draw(BackgroundLayer, MediumLayer, ForegroundLayer);
					break;
				case gameState.Running:
					game.Draw(BackgroundLayer, MediumLayer, ForegroundLayer);
					break;
			}

			BackgroundLayer.End();
			MediumLayer.End();
			ForegroundLayer.End();

			ForegroundLayer.Begin();
			// Mouse Cursor
			ForegroundLayer.Draw(Graphic.mousePointer, Control.MouseCoordinates, Color.White);
			ForegroundLayer.End();

			// XNA Standard
			base.Draw(gameTime);
		}
	}
}