using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.Runtime.Screens
{
    public abstract class Screen
    {
        public virtual bool isAlive { get; set; }

        public virtual bool isSilentAlive { get; set; }

        public abstract void PreDraw(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void PostDraw(GameTime gameTime);

        public abstract void DrawDirect(GameTime gameTime);

        public abstract void PreDrawDirect(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void Init();

        public void SetAlive()
        {
            isAlive = true;
        }

        public void SetDead()
        {
            isAlive = false;
        }

        public void SetSilentAlive()
        {
            isSilentAlive = true;
        }

        public void SetSilentDead()
        {
            isSilentAlive = false;
        }
    }
}
