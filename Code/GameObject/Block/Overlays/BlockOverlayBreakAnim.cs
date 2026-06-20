using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.Block
{
    public class BlockOverlayBreakAnim : BlockOverlay
    {
        public BlockOverlayBreakAnim(Block parent, Color c)
            : base(parent)
        {
            this.texture = Game1.thisGame.Content.Load<Texture2D>("Tiles/BreakHD1");
            this.c = c;
        }

        public void Draw(GameTime gameTime, int posX, int posY, int crack)
        {
            if (crack > 0)
                Game1.thisGame.spriteBatch.Draw(texture, new Rectangle(posX, posY, 16, 16), new Rectangle((crack-1) * 16, 0, 16, 16), c);
        }
    }
}
