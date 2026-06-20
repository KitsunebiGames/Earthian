using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Earthian.GameObject.Block.Blocks
{
    class BlockCobblestone : BlockBasic
    {
        public BlockCobblestone(bool wall)
            : base(2, wall)
        {
            this.SetTextureName("BlockStone");
            this.SetMaterial(BlockMaterial.STONE);
            this.name = "Cobblestone";
            this.textureSize = 16;
        }
    }
}
