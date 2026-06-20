using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.Item
{
    public class ItemStack
    {
        public Item item;
        public int count;

        public ItemStack(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }

		public void RemoveItem()
		{
			this.item = null;
			this.count = -1;
		}

        public float GetWeight()
        {
            return item.Weight * count;
        }

        public void Draw(GameTime gameTime, Rectangle target, Vector2 label)
        {
			if (count > 0)
			{
				item.Draw(gameTime, target);
				if (count > 1)
				{
					Game1.thisGame.spriteBatch.DrawString(Runtime.Runtime.thisRuntime.font, count.ToString(), label, Color.White);
				}
				if (Utilities.Input.GetMouseOver(target))
				{

					Game1.thisGame.spriteBatch.DrawString(Runtime.Runtime.thisRuntime.font, item.name, Utilities.Input.GetMousePos(), Color.White);
				}
				
			}
        }
    }
}
