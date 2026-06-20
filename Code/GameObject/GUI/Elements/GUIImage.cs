using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.GUI.Elements
{
    public class GUIImage : GUIElement
    {
        private Texture2D tex;

        public GUIImage(string texture)
            : base()
        {
            tex = Game1.thisGame.Content.Load<Texture2D>(texture);
            this.SizeX = tex.Width;
            this.SizeY = tex.Height;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.thisGame.spriteBatch.Draw(tex, this.GetBounds(), Color.White);
            base.Draw(gameTime);
        }
    }
}
