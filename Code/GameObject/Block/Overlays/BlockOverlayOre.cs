using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Block.Overlays
{
    class BlockOverlayOre : BlockOverlay
    {
        int ID;
        public BlockOverlayOre(Block parent, int ID) : base(parent)
        {
            this.ID = ID;
            SetTextureFromID();
        }

        public void SetTextureFromID()
        {
            if (ID == (int)Blocks.BlockOreIDS.COPPER)
                SetTextureName("CopperOre");
            if (ID == (int)Blocks.BlockOreIDS.IRON)
                SetTextureName("IronOre");
            if (ID == (int)Blocks.BlockOreIDS.SILVER)
                SetTextureName("SilverOre");
            if (ID == (int)Blocks.BlockOreIDS.GOLD)
                SetTextureName("GoldOre");
        }

        public void SetTextureName(string name)
        {
            this.texture = Game1.thisGame.Content.Load<Texture2D>("Tiles/Ores/" + name);
        }

        public override void OnParentDestroyed()
        {
            base.OnParentDestroyed();
        }
    }
}
