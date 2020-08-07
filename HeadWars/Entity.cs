using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeadWars
{
	// Class for all the entities
	abstract class Entity
	{
		// Variables
		/// The radius of the entity. Mutable. (Collision detection)
		public float radius = 20;
		/// Wheter the entity is/was destroyed or not
		public Boolean isDestroyed;
		public Vector2 position, velocity;
		public float angle;

		protected Texture2D sprite;
		/// Sprite color (Mostly for transparency stuff)
		protected Color color = Color.White;

		// Properties
		/// Gets the size of the sprite according to the image's size if it exists
		public Vector2 size
		{
			get
			{
				return sprite == null ? Vector2.Zero : new Vector2(sprite.Width, sprite.Height);
			}
		}

		// Methods
		/// Pushes the entity
		public virtual void Push(Entity e, int f = 10)
		{
			Vector2 d = position - e.position;
			velocity += f * d / (d.LengthSquared() + 1);
		}

		/// Update
		public abstract void Update();

		/// Draw
		public virtual void Draw(SpriteBatch BackgroundLayer)
		{
			BackgroundLayer.Draw(sprite, position, null, color, angle, size / 2f, 1f, 0, 0);
		}
	}
}