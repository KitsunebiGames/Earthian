using Earthian.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Block.Blocks
{
    class BlockRedCubeThingy : BlockMulti
    {
        public BlockRedCubeThingy(bool wall) : base(666, wall, new Microsoft.Xna.Framework.Rectangle(0,0, 64, 64))
        {
            this.texture = Game1.thisGame.Content.Load<Texture2D>("Items/Unknown");
            this.textureSize = 16;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }
    }
}
