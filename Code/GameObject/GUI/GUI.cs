using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.GUI
{
    public abstract class GUI
    {
        public bool hide = false;

        public abstract void Draw(GameTime gameTime);
        public virtual void PostDraw(GameTime gameTime)
        {

        }
        public abstract void Update(GameTime gameTime);
    }
}
