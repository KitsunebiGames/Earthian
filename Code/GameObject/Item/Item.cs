using Earthian.GameObject;
using Earthian.GameObject.Block;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Item
{
	public abstract class Item
	{
		protected Texture2D texture;
		protected Rectangle texturePos;
		private string textureName = "Items/Unknown";
		public string name = "???";
		public int id;
		private bool isConsumable;
		private float weight;
		public int stackSize = 50;
		public int swingTime = 1;
		public int toolPower = 0;
		public float scale = 1f;

		public float Weight
		{
			get { return this.weight; }
		}

		public bool Consumable
		{
			get { return this.isConsumable; }
		}

		public Item(int id)
		{
			this.id = id;
			texturePos = new Rectangle(0, 0, 16, 16);
			LoadTexture();
		}

		public Item(int id, bool isConsumable)
		{
			this.id = id;
			this.isConsumable = isConsumable;
		}

		public abstract void Update(GameTime gameTime);

		public abstract bool Use(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv);

		public abstract bool AltUse(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv);

		public void ForceUnconsumable()
		{
			isConsumable = false;
		}

		public void LoadTexture()
		{
			try
			{
				this.texture = Game1.thisGame.Content.Load<Texture2D>(textureName);
			}
			catch
			{
				Console.WriteLine("Failed to load sprite for: " + this.textureName);
				this.textureName = "Items/Unknown";
				this.texture = Game1.thisGame.Content.Load<Texture2D>(textureName);
				texturePos = new Rectangle(0, 0, texture.Width, texture.Height);
			}
		}

		public Texture2D GetTexture()
		{
			return texture;
		}

		public Item SetTextureName(string name)
		{
			this.textureName = name;
			LoadTexture();
			return this;
		}

		public Item SetStackSize(int n)
		{
			this.stackSize = n;
			return this;
		}

		public void Draw(GameTime gameTime, Rectangle target)
		{
			Game1.thisGame.spriteBatch.Draw(this.texture, target, this.texturePos, Color.White);
		}

		public Rectangle DrawSwingAnimation(GameTime gameTime, int swingTime, Vector2 center, bool flip, Entity.Entities.EntityPlayer player)
		{
			int r = 32; //distance of swing from centre of player
			float s = 2f; //how far the swing swings
			float a;
			a = 180 - (float)(s * (float)swingTime / (float)this.swingTime);
			int x, y;
			int m = flip ? -1 : 1;
			x = (int)(center.X + Math.Sin(a) * r * m);
			y = (int)(center.Y + Math.Cos(a) * r);

			Rectangle target;

			if (player.doFlip)
			{
				target = new Rectangle(x, y, (int)(16 * scale), (int)(16 * scale));
				Game1.thisGame.spriteBatch.Draw(this.texture, target, this.texturePos, Color.White, 0 + (a * m * -1), texturePos.Center.ToVector2(), SpriteEffects.FlipVertically, 1f);
			}
			else
			{
				target = new Rectangle(x, y, (int)(16 * scale), (int)(16 * scale));
				Game1.thisGame.spriteBatch.Draw(this.texture, target, this.texturePos, Color.White, 180 + (a * m * -1), texturePos.Center.ToVector2(), SpriteEffects.None, 1f);
			}
			return target;
		}

		public float GetToolPowerVsMaterial(Block.Blocks.BlockMaterial material)
		{
			return 1f;
		}

		public bool Compare(Item other)
		{
			return (other != null && other.id == this.id && other.name == this.name);
		}
	}
}
