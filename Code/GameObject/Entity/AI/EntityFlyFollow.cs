using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Entity.AI
{
	public class EntityFlyFollow : EntityAI
	{
		public float distance = 200f;
		public float speed = 0.6f;
		private bool nc;
		private bool doFollow = true;

		public EntityFlyFollow(float speed, bool noclip, float distance)
		{
			this.speed = speed;
			this.nc = noclip;
			this.distance = distance;
		}

		public override void Handle(Entity actor, Entity target)
		{
			if (doFollow)
			{
				if (target == null)
					return;
				if (distance < 0 || Microsoft.Xna.Framework.Vector2.Distance(actor.Position, target.Position) < distance)
				{
					if (actor.Centre().X < target.Centre().X)
						actor.MotionX += speed;
					else
						actor.MotionX -= speed;
					if (actor.Centre().Y < target.Centre().Y)
						actor.MotionY += speed;
					else
						actor.MotionY -= speed;
				}
			}
		}

		public void SetFollow(bool value)
		{
			doFollow = value;
		}
	}
}
