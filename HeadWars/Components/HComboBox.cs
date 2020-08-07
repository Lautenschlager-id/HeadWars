using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// A button
	class HComboBox
	{
		// Variables
		public Boolean isWorking = true;
		public string Value;
		public Boolean ValueHasChanged = false;

		Boolean isOpen = false;
		List<string> sourceData;
		List<HSquare> squares = new List<HSquare>();

		// Methods
		/// Constructor
		public HComboBox(List<string> dataSource, int x = 10, int y = 10, int initialValue = 0, int width = 100, int height = 30)
		{
			sourceData = dataSource;
			Value = dataSource[initialValue];

			// Button
			squares.Add(new HSquare(Sprite.GetGraphic(Value), x, y, width, height));

			// Data
			for (int i = 0; i < dataSource.Count; i++)
			{
				squares.Add(new HSquare(Sprite.GetGraphic(dataSource[i]), x, y + (height + 1) * (i + 1), width, height));
				squares[squares.Count - 1].setColor(Color.Gray.Collection());
				squares[squares.Count - 1].opacity = .5f;
				squares[squares.Count - 1].isWorking = false;
				squares[squares.Count - 1].setHoverSound = Sound.buttonHover;
				squares[squares.Count - 1].setClickSound = Sound.buttonClick;
			}
		}

		/// Enables/disables the data display
		private void DataState(Boolean working)
		{
			for (int i = 1; i < squares.Count; i++)
				squares[i].isWorking = working;
		}

		/// Update
		public void Update()
		{
			if (isWorking)
			{
				foreach (HSquare s in squares)
					s.Update();

				if (squares[0].onClick)
				{
					isOpen = !isOpen;

					DataState(isOpen);

					return;
				}

				for (int i = 1; i < squares.Count; i++)
					if (squares[i].isWorking)
					{
						squares[i].opacity = squares[i].MouseHover ? 1f : .5f;

						if (squares[i].onClick)
						{
							Value = sourceData[i - 1];

							squares[0].setContent(Sprite.GetGraphic(Value));

							DataState(isOpen = false);

							ValueHasChanged = true;

							break;
						}
					}
			}
		}

		/// Draw
		public void Draw(SpriteBatch Layer)
		{
			if (isWorking)
			{
				foreach (HSquare s in squares)
					s.Draw(Layer);
			}
		}
	}
}