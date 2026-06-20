using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Entity.AI
{
    public class EntityAIDeceleration : EntityAI
    {
        private float decelX, decelY;
        private float maxX, maxY;
        private bool grav;

        public EntityAIDeceleration(float dX, float mX, float dY, float mY, bool gravity)
        {
            decelX = dX;
            maxX = mX;
            decelY = dY;
            maxY = mY;
            grav = gravity;
        }

        public override void Handle(Entity actor, Entity target)
        {
            float y = actor.MotionY;
            if (!grav)
                y = Math.Abs(y);
            if (Math.Abs(actor.MotionX) > maxX)
                actor.MotionX *= decelX;
            if (Math.Abs(y) > maxY)
                actor.MotionY *= decelY;
        }
    }
}
