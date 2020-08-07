using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The initial credits before launching the game itself
	class Presentation
	{
		// Consts
		const float updateIteration = .02f;
		const int nextImage = 6;

		// Variables
		int totalTextures;
		float nextImageTimer = 0;
		int currentImage = 0;

		Texture2D[] textureContent;
		Rectangle fullscreenRect = new Rectangle(0, 0, (int)HeadWars.ScreenDimension.X, (int)HeadWars.ScreenDimension.Y);

		// Methods
		/// Constructor
		public Presentation()
		{
			MediaPlayer.Play(Sound.MenuMusic);

			textureContent = Graphic.presentation;

			totalTextures = textureContent.Length;
		}

		/// Update
		public void Update()
		{
			nextImageTimer += updateIteration;
			if ((int)Math.Floor(nextImageTimer) >= nextImage)
			{
				currentImage++;
				nextImageTimer = 0;
			}
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer)
		{
			if (currentImage < totalTextures)
			{
				float alpha = (1 / (nextImageTimer + .05f));
				BackgroundLayer.Draw(textureContent[currentImage], fullscreenRect, Color.White * alpha);
			}
			else
				HeadWars.currentGameState = HeadWars.gameState.Login;
		}
	}
}