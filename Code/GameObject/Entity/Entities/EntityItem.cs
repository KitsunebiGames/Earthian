using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.GameObject.Item;
using Earthian.GameObject.World;
using Earthian.GameObject.Entity.AI;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.Entity.Entities
{

	public class EntityItem : Entity
	{
		public static int id = 1;
		private static Random rand = new Random();
		private World.World parent;
		private int pickupTimer;
		EntityFlyFollow follow;
		private ItemStack item;

		public EntityItem(ItemStack item)
			: base(EntityItem.id)
		{
			this.item = item;
			follow = new AI.EntityFlyFollow(0.4f, true, 200f);
			this.ai.Add(follow);
			//this.ai.Add(new AI.EntityAIGravity());
			this.PhysicsData.GravityOn = true;
			this.PhysicsData.Gravity = 0.1f;
			width = 16;
			height = 16;
		}

		public override void Draw(GameTime gameTime)
		{
			Game1.thisGame.spriteBatch.End();
			Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Runtime.Runtime.thisRuntime.mCamera.Transform);
			this.item.item.Draw(gameTime, this.Hitbox);
		}

		public override void Update(GameTime gameTime)
		{
			if (!IsDead && ShouldLoad())
			{
				this.pickupTimer -= gameTime.ElapsedGameTime.Milliseconds;
				HandlePhysics(gameTime);
			}
		}

		public new void HandlePhysics(GameTime time)
		{
			if (pickupTimer <= 0)
				follow.SetFollow(true);
			else
				follow.SetFollow(false);
			if (Vector2.Distance(((Entities.EntityPlayer)this.parent.entities[0]).Position, Position) < 1024f) Noclip = true;
			else Noclip = false;

			Target = parent.entities[0];
			MotionX *= 0.95f;
			HandleAI();
			if (this.Hitbox.Intersects(World.entities[0].Hitbox) && pickupTimer <= 0)
			{
				if (((Entities.EntityPlayer)this.parent.entities[0]).Inventory.AddItemStack(this.item))
					this.IsDead = true;
			}
			Physics.HandleEntityPhysics(time, this);
		}

		public void SetPickupTimer(int time)
		{
			pickupTimer = time;
		}

		public override void SpawnEntity(Entity entity, World.World world, Vector2 position)
		{
			this.World = world;
			this.parent = world;
			this.Position = position;
			this.pickupTimer = 200;
			this.IsDead = false;
		}

		public override void Hit(int baseDmg, Entity source, Object damageType)
		{
			this.IsDead = true;
		}

		public override void UseBlock(Block.Block block, int x, int y, int z, World.World world)
		{
		}

		public override void Init()
		{
		}
	}
}
