using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Entity.AI
{
    public class TargeterPlayer : EntityAI
    {
        private float distance;

        public TargeterPlayer(float distance)
        {
            this.distance = distance;
        }

        public override void Handle(Entity actor, Entity target)
        {
            actor.Target = actor.World.player;
            //find players within radius later
        }
    }
}
