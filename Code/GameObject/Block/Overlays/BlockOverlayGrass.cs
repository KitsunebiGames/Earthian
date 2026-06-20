using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.Block
{
    public class BlockOverlayGrass : BlockOverlay
    {
        public BlockOverlayGrass(Block parent, Color c)
            : base(parent)
        {
            this.texture = Game1.thisGame.Content.Load<Texture2D>("Tiles/GrassOverlay");
            this.c = c;
            this.Drops = null;
        }

        public override void OnParentDestroyed()
        {
            
        }
    }
}
