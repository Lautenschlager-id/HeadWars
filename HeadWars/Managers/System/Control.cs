using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HeadWars
{
	// Keyboard, Mouse controls
	class Control
	{
		// Variables
		/// Keyboard
		private static KeyboardState keyboardCurrent, keyboardLast;
		private static Keys[] AimingKeys = new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down };
		private static Keys[] ControlKeys = new Keys[] { Keys.A, Keys.D, Keys.W, Keys.S };
		/// Mouse
		private static MouseState mouseCurrent, mouseLast;
		private static Boolean usingMouse = false;

		// Properties
		public static Vector2 MouseCoordinates
		{
			get
			{
				return new Vector2(mouseCurrent.X, mouseCurrent.Y);
			}
		}
		public static Boolean MouseClicked
		{
			get
			{
				return mouseLast.LeftButton == ButtonState.Pressed && mouseCurrent.LeftButton == ButtonState.Released;
			}
		}
		public static Boolean MouseClicking
		{
			get
			{
				return mouseCurrent.LeftButton == ButtonState.Pressed;
			}
		}
		public static Boolean Shift
		{
			get
			{
				return HoldingKey(Keys.LeftShift) || HoldingKey(Keys.RightShift);
			}
		}

		// Functions
		/// Keyboard
		public static Boolean KeyDown(Keys key)
		{
			// If the last key is up and a new one is down (can't repeat the same key so it fix bugs)
			return keyboardLast.IsKeyUp(key) && keyboardCurrent.IsKeyDown(key);
		}
		public static Boolean KeyUp(Keys key)
		{
			return keyboardLast.IsKeyDown(key) && keyboardCurrent.IsKeyUp(key);
		}
		public static Boolean HoldingKey(Keys key)
		{
			return keyboardCurrent.IsKeyDown(key);
		}

		public static Vector2 getMoveCoordinate()
		{
			Vector2 dir = Vector2.Zero;

			if (keyboardCurrent.IsKeyDown(ControlKeys[0]))
				dir.X -= 1;
			if (keyboardCurrent.IsKeyDown(ControlKeys[1]))
				dir.X += 1;
			if (keyboardCurrent.IsKeyDown(ControlKeys[2]))
				dir.Y -= 1;
			if (keyboardCurrent.IsKeyDown(ControlKeys[3]))
				dir.Y += 1;

			if (dir.LengthSquared() > 1)
				dir.Normalize();

			return dir;
		}
		public static Vector2 getAimCoordinate()
		{
			Vector2 dir;

			if (usingMouse)
				dir = MouseCoordinates - Player.Instance.position;
			else
			{
				dir = Vector2.Zero;

				if (keyboardCurrent.IsKeyDown(AimingKeys[0]))
					dir.X -= 1;
				if (keyboardCurrent.IsKeyDown(AimingKeys[1]))
					dir.X += 1;
				if (keyboardCurrent.IsKeyDown(AimingKeys[2]))
					dir.Y -= 1;
				if (keyboardCurrent.IsKeyDown(AimingKeys[3]))
					dir.Y += 1;
			}

			return dir == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(dir);
		}

		// Methods
		/// Reverses the movement and aiming controls
		public static void MoveToAim()
		{
			Keys[] Move = ControlKeys;
			Keys[] Aim = AimingKeys;

			ControlKeys = Aim;
			AimingKeys = Move;
		}

		/// Update
		public static void Update()
		{
			keyboardLast = keyboardCurrent;
			mouseLast = mouseCurrent;

			keyboardCurrent = Keyboard.GetState();
			mouseCurrent = Mouse.GetState();

			// Aiming system (Mouse or Arrows)
			// If you are holding a directional key
			if (AimingKeys.Any(key => keyboardCurrent.IsKeyDown(key)))
				usingMouse = false;
			// If he moved the mouse
			else if (MouseCoordinates != new Vector2(mouseLast.X, mouseLast.Y))
				usingMouse = true;
		}
	}
}