using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Spire.Barcode;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace HeadWars
{
	// The login window
	class Code
	{
		// Variables
		List<HTextBox> textboxes = new List<HTextBox>(1);
		List<HButton> buttons = new List<HButton>(2);
		List<HLabel> labels = new List<HLabel>(2);

		string message;

		Texture2D QRCodeTexture;
		Microsoft.Xna.Framework.Rectangle QrCodeRectangle;

		// Methods
		/// Constructor
		public Code()
		{
			// Resizes the window
			HeadWars.Resize(320, 300);

			labels.Add(new HLabel(Translation.GetString("secret_code"), Font.WindowTitle));
			labels[0].setPosition(0, 0, "xcenter top");

			// Code
			textboxes.Add(new HTextBox(0, 0, 17, false, 250, 45));
			textboxes[0].setFont(Font.MediumTextBold);
			textboxes[0].setPosition(0, 40, "xcenter ycenter");

			buttons.Add(new HButton(Translation.GetString("validate"), 0, 0));
			buttons[0].setPosition(0, 90, "xcenter ycenter");

			labels.Add(new HLabel(message = "", Font.Text));
			labels[1].setPosition(0, 140, "xcenter");

			buttons.Add(new HButton(Translation.GetString("button_back"), 0, 0));
			buttons[1].setPosition(0, 0, "left bottom");

			// QR Code
			string data = "";
			DatabaseManager.Connect();
			data = DatabaseManager.getCode();
			DatabaseManager.Disconnect();

			if (data == null)
			{
				QRCodeTexture = Graphic.no_code;
				QrCodeRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, QRCodeTexture.Width, QRCodeTexture.Height);
			}
			else
			{
				BarcodeSettings settings = new BarcodeSettings
				{
					Unit = GraphicsUnit.Pixel,
					Type = BarCodeType.QRCode,
					ResolutionType = ResolutionType.UseDpi,
					BackColor = System.Drawing.Color.White,
					ForeColor = System.Drawing.Color.Black,
					X = 3,
					Data = data
				};

				BarCodeGenerator generator = new BarCodeGenerator(settings);
				QRCodeTexture = bitmapToTexture2D((Bitmap)generator.GenerateImage());
				QrCodeRectangle = new Microsoft.Xna.Framework.Rectangle(3, 30, QRCodeTexture.Width - 5, QRCodeTexture.Height - 30);
			}
		}

		/// Bitmap to Texture2D
		private Texture2D bitmapToTexture2D(Bitmap bmp)
		{
			int[] imageData = new int[bmp.Width * bmp.Height];
			Texture2D texture = new Texture2D(HeadWars.Instance.GraphicsDevice, bmp.Width, bmp.Height);

			unsafe
			{
				BitmapData originData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				uint* byteData = (uint*)originData.Scan0;

				// BGRA => RGBA
				for (int i = 0; i < imageData.Length; i++)
					byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);

				Marshal.Copy(originData.Scan0, imageData, 0, bmp.Width * bmp.Height);
				byteData = null;
				bmp.UnlockBits(originData);
			}

			texture.SetData(imageData);

			return texture;
		}

		/// Update
		public void Update()
		{
			foreach (HTextBox b in textboxes)
				b.Update();
			foreach (HButton b in buttons)
				b.Update();
			foreach (HLabel l in labels)
				l.Update();

			if (buttons[0].onClick)
			{
				if (textboxes[0].answerText.Length > 0)
				{
					DatabaseManager.Connect();
					int prize = DatabaseManager.getPrizeCode(textboxes[0].answerText);
					if (prize > 0)
					{
						Info.totalExperience += prize;
						DatabaseManager.removePrizeCode(textboxes[0].answerText);
						SkillManager.updateLeveling();

						message = string.Format(Translation.GetString("winExperience"), prize);
						labels[1].textColor(Microsoft.Xna.Framework.Color.Gold);
					}
					else
					{
						message = Translation.GetString("codeerror_invalid");
						labels[1].textColor(Microsoft.Xna.Framework.Color.Yellow);
					}
					DatabaseManager.Disconnect();

					textboxes[0].answerText = "";
				}
			}
			else if (buttons[1].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Menu;
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

			if (message != "")
			{
				labels[1].text = message;
				labels[1].setPosition(0, 140, "xcenter");
			}

			foreach (HLabel l in labels)
				l.Draw(MediumLayer);

			BackgroundLayer.Draw(QRCodeTexture, new Vector2(HeadWars.ScreenDimension.X / 2 - QrCodeRectangle.Width / 2, 65), QrCodeRectangle, Microsoft.Xna.Framework.Color.White);
		}
	}
}