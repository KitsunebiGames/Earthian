using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.Block.Blocks
{
    class BlockDirt : BlockBasic
    {
        public BlockDirt(bool wall)
            : base(3, wall)
        {
            this.SetTextureName("BlockDirt_");
            this.SetMaterial(BlockMaterial.DIRT);
            this.name = "Dirt";
            this.textureSize = 8;
        }

        public override void InitOverlays()
        {
            base.InitOverlays();
            if (state != 15)
                this.overlays.Add(new BlockOverlayGrass(this, Color.LimeGreen * 2));
        }

        private static Color defaultGrass = new Color(45, 230, 25);
    }
}
