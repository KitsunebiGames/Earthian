using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Earthian.GameObject.Item;

namespace Earthian.GameObject.Block
{
    public class BlockOverlay
    {
        public Color c = Color.White;
        public virtual Texture2D texture { get; set; }
        public ItemStack Drops;
        private Block parent;

        public BlockOverlay(Block parent)
        {
            this.parent = parent;
        }

        public void Draw(GameTime time, int posX, int posY, TileTexturePosition target)
        {
			Game1.thisGame.spriteBatch.Draw(texture, parent.hitbox, new Rectangle(target.X * 8, target.Y * 8, 8, 8), c);
        }

        public virtual void OnParentDestroyed()
        {
            parent.GetWorld().SpawnEntity(new Entity.Entities.EntityItem(Drops), new Vector2(parent.BlockPosition.X, parent.BlockPosition.Y));
        }
    }
}
