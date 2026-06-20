using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Entity.AI
{
    public abstract class EntityAI
    {
        public abstract void Handle(Entity actor, Entity target);
    }
}
