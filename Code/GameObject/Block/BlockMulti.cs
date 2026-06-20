using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Block
{
    public abstract class BlockMulti : Block
    {
        Rectangle size;

        public BlockMulti(int id, bool wall, Rectangle size) : base(id, wall)
        {
            this.size = size; //does not matter.
        }

        public override void Draw(GameTime gameTime)
        {
            //Bla bla, you know the drill
            if (!shouldDraw) return;
            //Draws the object with the positions specified in {Rectangle: size}
            Game1.thisGame.spriteBatch.Draw(texture, new Rectangle(PosX - (textureSize / 2), PosY - size.Height, size.Width, size.Height), new Rectangle(target.X * textureSize, target.Y * textureSize, textureSize, textureSize), new Color(darkness, darkness, darkness, 255));
            foreach (BlockOverlay o in this.overlays)
            {
                //Draw all overlays ontop of it, if neccesary.
                o.Draw(gameTime, PosX, PosY, target);
            }
        }

    }
}
