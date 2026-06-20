using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.GameObject.World;
using Earthian.GameObject.Entity.AI;

namespace Earthian.GameObject.Entity
{
	public class EntityPhysicsData
	{
		public float Gravity { get; set; }

		public bool CanNoClip { get; set; }

		public bool GravityOn { get; set; }
	}

	public class Physics
	{
		public static void HandleEntityPhysics(GameTime gameTime, Entity actor)
		{
			float v;
			actor.posX += actor.MotionX;
			actor.posX = (float)Math.Round(actor.posX);
			if (!actor.Noclip)
			{
				foreach (BlockPos pos in actor.World.GetPositionBlock(actor.Position).GetAdjacentSides(actor.width / 4, actor.height / 4))
				{
					Block.Block b = actor.World.GetBlock(pos);
					if (b != null)
					{
						Rectangle blockAABB = b.hitbox;
						v = CalculateAABBCollisionX(actor.Hitbox, blockAABB);
						if (v != 0.0f)
						{
							actor.MotionX = 0f;
							actor.posX += v;
						}
					}
				}
			}
			if (actor.PhysicsData != null)
			{
				actor.MotionY += actor.PhysicsData.Gravity;
			}
			actor.posY += actor.MotionY;
			if (!actor.Noclip)
			{
			//actor.posY = (float)Math.Round(actor.posY);
			actor.IsGrounded = false;
			foreach (BlockPos pos in actor.World.GetPositionBlock(actor.Position).GetAdjacentSides(actor.width / 4, actor.height / 4))
			{
				Block.Block b = actor.World.GetBlock(pos);
				if (b != null)
				{
					Rectangle blockAABB = b.hitbox;
					v = CalculateAABBCollisionY(actor.Hitbox, blockAABB);

						if (v < 0)
						{
							actor.posY -= actor.MotionY;
							actor.MotionY = 0;
							actor.IsGrounded = true;
						}
						if (v > 0)
						{
							actor.posY += v;
							actor.MotionY = 0.5f;
						}
					}
				}
			}
			if (actor.IsGrounded && actor.IsJumping)
			{
				actor.MotionY -= 15f; //replace with actor.CalculateJumpPower()
			}

			/*float v, x, y;
			x = y = 0;
			actor.posX += actor.MotionX;
			actor.posX = (float)Math.Round(actor.posX);
			foreach (BlockPos pos in actor.World.GetPositionBlock(actor.Position).GetAdjacentSides(actor.width / 4, actor.height / 4))
			{
				Block.Block b = actor.World.GetBlock(pos);
				if (b != null)
				{
					Rectangle blockAABB = b.hitbox;
					v = CalculateAABBCollisionX(actor.Hitbox, blockAABB);
					x = v;
					if (v != 0.0f)
					{
						actor.MotionX = 0f;
						actor.posX = (float)Math.Round(actor.posX);
					}
				}
			}
			actor.posY += actor.MotionY;
			actor.posY = (float)Math.Round(actor.posY);
			actor.IsGrounded = false;
			foreach (BlockPos pos in actor.World.GetPositionBlock(actor.Position).GetAdjacentSides(actor.width / 4, actor.height / 4))
			{
				Block.Block b = actor.World.GetBlock(pos);
				if (b != null)
				{
					Rectangle blockAABB = b.hitbox;
					v = CalculateAABBCollisionY(actor.Hitbox, blockAABB);
					y += v;
					if (v < 0)
					{
						actor.posY -= actor.MotionY;
						actor.IsGrounded = true;
					}
					if (v != 0.0f)
					{
						actor.MotionY = 0;
					}
				}
			}
			if (actor.IsGrounded && actor.IsJumping)
			{
				actor.MotionY -= 15f; //replace with actor.CalculateJumpPower()
			}
			actor.posX += x;
			actor.posY += y;*/
		}

		public static float CalculateAABBCollisionX(Rectangle a, Rectangle b)
		{
			if (a.Intersects(b) || b.Intersects(a))
			{
				if (a.Center.X < b.Center.X)
				{
					return ((float)b.Left - a.Right);
				}
				return ((float)b.Right - a.Left);
			}
			return 0.0f;
		}

		public static float CalculateAABBCollisionY(Rectangle a, Rectangle b)
		{
			if (a.Intersects(b) || b.Intersects(a))
			{
				if (a.Center.Y < b.Center.Y)
				{
					return ((float)b.Top - a.Bottom) / 4f;
				}
				return ((float)b.Bottom - a.Top);
			}
			return 0.0f;
		}
	}
}
