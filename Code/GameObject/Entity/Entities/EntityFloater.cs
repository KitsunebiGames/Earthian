using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.Entity.Entities
{
	public class EntityFloater : Entity
	{

		public EntityFloater()
			: base(3)
		{
			ai.Add(new AI.TargeterPlayer(0f));
			ai.Add(new AI.EntityFlyFollow(0.04f, false, -1f));
			width = 28;
			height = 40;
			this.PhysicsData = null;
		}

		public override void Draw(GameTime gameTime)
		{
			anim.Draw(gameTime, this.Hitbox, this.MotionX < 0);
		}

		public override void Init()
		{
			anim = new Utilities.EntityAnimationHandler(1);
			anim.addAnim(0, Game1.thisGame.Content.Load<Texture2D>("Entities/Floater/EntityFloater"), 4, new Utilities.EntityTexturePosition(14, 20), 100);
			anim.changeState(0, true);
		}

		public new void HandlePhysics(GameTime gameTime)
		{
			HandleAI();
			this.Position = new Vector2(Position.X + MotionX, Position.Y + MotionY);
			/*
            Physics.CollisionEvent c = Physics.GetDirectionalCollision(gameTime, this);
            if (c.HasCollision())
            {
                int l = c.Left() ? -1 : c.Right() ? 1 : 0;
                int u = c.Up() ? -1 : c.Down() ? 1 : 0;
                this.MotionX *= l * -1;
                this.MotionY *= u * -1;
                this.Position = new Vector2(oldPos.X - l,oldPos.Y - u);
            }*/
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			anim.Update(gameTime);
			CheckHit(gameTime);
		}

		public void CheckHit(GameTime gameTime)
		{
			EntityPlayer player = (EntityPlayer)this.World.player;
			if (player.swingTime > 0 && player.swingingHitbox.Intersects(this.Hitbox))
			{
				this.Hit(player.Damage, player, null);
			}
		}

		public override void UseBlock(Block.Block block, int x, int y, int z, World.World world)
		{
			throw new NotImplementedException();
		}

		public override void SpawnEntity(Entity entity, World.World world, Vector2 position)
		{
			entity.World = world;
			entity.Position = position;
			entity.IsDead = false;
		}
	}
}
