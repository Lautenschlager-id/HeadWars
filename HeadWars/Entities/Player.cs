using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeadWars
{
	// The player entity
	class Player : Entity
	{
		// Variables
		public float bulletDiscrepancy = 0.04f;
		public float currentTime = 0;
		public SkillManager.Skill shootTimer = SkillManager.skillData["shootSpeed"];
		public SkillManager.Skill bulletAmount = SkillManager.skillData["bullet"];
		public SkillManager.Skill moveSpeed = SkillManager.skillData["velocity"];

		private static Dictionary<string, Keys> ActionKeys = new Dictionary<string, Keys>(){
			{"asteroid", Keys.Space},
			{"freeze", Keys.F},
			{"invisibility", Keys.I}
		};
		private static Player instance;
		/// Timer when the player is spawning
		private int loadingFrame = 0;
		/// Receives the value of (int)shootTimer.Value
		private int shootCooldown = 0;
		/// Bullet
		private int alterPosition = 1;
		// Asteroids
		private SkillManager.Skill asteroid = SkillManager.skillData["asteroid"];
		private int asteroidsShootable;
		// Freeze enemies
		private SkillManager.Skill freezeEnemy = SkillManager.skillData["freeze"];
		private Boolean hasFrozen = false;
		// Becomes untouchable
		private SkillManager.Skill invisibility = SkillManager.skillData["invisibility"];
		private float invisibilityTime = 0;
		private Boolean hasActivatedInvisibility = false;
		// Teleport
		private SkillManager.Skill teleport = SkillManager.skillData["teleport"];
		private Boolean hasTeleported = false;

		// Properties
		public static Player Instance
		{
			get
			{
				// Can't use the short-circuiting because the use of the instance variable is needed
				if (instance == null)
					instance = new Player();

				return instance;
			}
		}
		public static Boolean Exists
		{
			get
			{
				return instance != null;
			}
		}
		public Boolean isDead
		{
			get
			{
				return loadingFrame > 0;
			}
		}
		public Boolean isInvisible
		{
			get
			{
				return invisibilityTime > 0;
			}
		}

		// Methods
		/// Constructor
		public Player()
		{
			sprite = CharacterSelector.sprites[0][0];
			radius = 10;
			angle = -MathHelper.PiOver2;

			// Centers the player in the screen
			position = HeadWars.ScreenDimension / 2;

			asteroidsShootable = (int)asteroid.Value;
		}

		public void Reset()
		{
			sprite = CharacterSelector.sprites[0][0];
			position = HeadWars.ScreenDimension / 2;
			angle = -MathHelper.PiOver2;
			color = Color.White;

			currentTime = 0;
			loadingFrame = 10;
			shootCooldown = 0;
			alterPosition = 1;
			asteroidsShootable = (int)asteroid.Value;
			hasFrozen = false;
			invisibilityTime = 0;
			hasActivatedInvisibility = false;
			hasTeleported = false;

			EntityManager.freezeEnemy = 0;
		}

		public void Kill()
		{
			if (!isDead)
			{
				Score.Damage();

				EntityManager.New(new Explosion(Graphic.explosion, position, 100f));

				EntityManager.resetEntities();
				Spawner.restart();

				loadingFrame = 100;

				Sound.playerExplosion.Play(.8f, Maths.random.RandomRange(-.2f, .2f), 0);
			}
		}

		/// Update
		public override void Update()
		{
			if (isDead)
			{
				if (--loadingFrame <= 0 && !Score.gameOver)
				{
					position = HeadWars.ScreenDimension / 2;
					angle = -MathHelper.PiOver2;

					Sound.playerSpawn.Play(.6f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
			}
			else
			{
				currentTime += (float)HeadWars.GameTime.ElapsedGameTime.TotalSeconds;

				velocity += ((int)moveSpeed.Value / (Control.Shift ? 2 : 1)) * Control.getMoveCoordinate();

				position += velocity;
				position = Vector2.Clamp(position, size / 2, HeadWars.ScreenDimension - size / 2);

				// Rotates player to the moving direction
				if (velocity.LengthSquared() > 0)
					angle = velocity.Angle();

				// Resets the velocity
				velocity = Vector2.Zero;

				// Bullet system
				ShootBullet();

				// Skills
				if (invisibilityTime > 0)
				{
					invisibilityTime -= .008f;
					if (invisibilityTime <= 0)
					{
						color = Color.White;
						Sound.skillInvisibility.Play(.9f, Maths.random.RandomRange(-.2f, .2f), 0);
					}
				}

				/// Asteroid
				if (asteroid.isActive && Control.KeyUp(ActionKeys["asteroid"]))
					ShootAsteroid();
				/// Freeze
				if (freezeEnemy.isActive && Control.KeyUp(ActionKeys["freeze"]) && !hasFrozen)
				{
					hasFrozen = true;
					EntityManager.freezeEnemy = (float)freezeEnemy.Value;
					Sound.skillFreeze.Play(1f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
				// Invisibility
				if (invisibility.isActive && Control.KeyUp(ActionKeys["invisibility"]) && !hasActivatedInvisibility)
				{
					hasActivatedInvisibility = true;
					invisibilityTime = (float)invisibility.Value;
					color = Color.White * .6f;
					Sound.skillInvisibility.Play(.9f, Maths.random.RandomRange(-.2f, .2f), 0);
				}
				// Teleport
				if (teleport.isActive && Control.Shift && Control.MouseClicked && !hasTeleported)
				{
					hasTeleported = true;
					position = Control.MouseCoordinates;
					Sound.skillTeleport.Play(.9f, Maths.random.RandomRange(-.2f, .2f), 0);
				}

				if (shootCooldown > 0)
					shootCooldown--;
			}
		}

		/// Draw
		public override void Draw(SpriteBatch BackgroundLayer)
		{
			if (!isDead)
				base.Draw(BackgroundLayer);
		}

		// Regions
		#region Actions
		public void ShootBullet()
		{
			Vector2 dir = Control.getAimCoordinate();
			if (dir.LengthSquared() > 0 && shootCooldown <= 0)
			{
				shootCooldown = (int)shootTimer.Value;
				float aimAngle = dir.Angle();

				// Creates an angle discrepancy for the bullet
				float angleDiscrepancy = Maths.random.RandomRange(-bulletDiscrepancy, bulletDiscrepancy) + Maths.random.RandomRange(-bulletDiscrepancy, bulletDiscrepancy);
				Vector2 vel = Maths.polarToVector(aimAngle + angleDiscrepancy, 11f);

				// Rotate the initial position of the object in the direction it's moving
				Quaternion eulerAim = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

				// Bullets
				for (int q = 0; q < (int)bulletAmount.Value; q++)
				{
					int y = (q + 1) * (8 + q);
					Vector2 offset = Vector2.Transform(new Vector2(50, y * alterPosition), eulerAim);
					EntityManager.New(new Bullet(position + offset, vel));
					alterPosition *= -1;
				}
			}
		}

		public void ShootAsteroid()
		{
			if (--asteroidsShootable >= 0)
			{
				Quaternion eulerAim = Quaternion.CreateFromYawPitchRoll(0, 0, angle);
				Vector2 offset = Vector2.Transform(new Vector2(40, -25), eulerAim);

				EntityManager.New(new Asteroid(position + offset));
			}
		}
		#endregion
	}
}