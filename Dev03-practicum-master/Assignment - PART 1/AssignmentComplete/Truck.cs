using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentComplete
{
	class Truck : ITruck
	{
		Vector2 position;
		bool owner;
		Texture2D truckdraw;
		IContainer container;
		Vector2 velocity;

		public Truck(Vector2 position, Vector2 velocity, Texture2D truckdraw, bool owner)
		{
			this.truckdraw = truckdraw;
			this.owner = owner;
			if (this.owner)
			{
				this.position = position + new Vector2(100, 35);
			}
            // positie voor ikea trucks
            else
            {
                this.position = position + new Vector2(-100, 0);
            }
			this.velocity = velocity;
		}

		public IContainer Container
		{
			get
			{
				return container;
			}
			set
			{
				container = value;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
		}

		public Vector2 Velocity
		{
			get
			{
				return velocity;
			}
		}

		public void AddContainer(IContainer container)
		{
			this.Container = container;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (container != null)
			{
				this.container.Draw(spriteBatch);
			}

			if (this.owner == true)
			{
				spriteBatch.Draw(this.truckdraw, this.position, Color.HotPink);
			}
			else
			{
				spriteBatch.Draw(this.truckdraw, this.position, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0.0f);
			}
		}

		public void StartEngine()
		{
			if (owner == true)
			{
				velocity = new Vector2(40, 0);
			}

			else
			{
				velocity = new Vector2(-40, 0);
			}
		}

		public void Update(float dt)
		{
			if (owner == true)
			{
				this.position = this.position + (this.velocity * dt);
				this.container.Position = this.position + new Vector2(-12, -12);
			}

			else
			{
				this.position = this.position + (this.velocity * dt);
				this.container.Position = this.position + new Vector2(32, -20);
			}
		}
	}

}
