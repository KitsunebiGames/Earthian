using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Entity.AI
{
    public class EntityAIGravity : EntityAI
    {
        public bool enabled = true;

        public override void Handle(Entity actor, Entity target)
        {
            if (enabled)
            {
                actor.MotionY += actor.World.GetGravity();
            }
        }

        public void SetEnabled(bool e)
        {
            enabled = e;
        }
    }
}
