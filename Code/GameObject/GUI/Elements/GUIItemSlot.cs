using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Earthian.GameObject.Item;
using Earthian.GameObject.Container;

namespace Earthian.GameObject.GUI.Elements
{
	public class GUIItemSlot : GUIElement
	{
		protected static Texture2D slotTexture;
		public static int slotSize = 64;
		public static int itemSize = 32;

		private Container.Container inventory;
		private int slot;

		public GUIItemSlot(Container.Container inventory, int slot)
		{
			locked = true;
			this.slot = slot;
			this.inventory = inventory;

			this.SizeX = slotSize;
			this.SizeY = slotSize;

			if (slotTexture == null)
				slotTexture = Game1.thisGame.Content.Load<Texture2D>("GUI/Slot"); 
		}

		public override void Draw(GameTime gameTime)
		{
			ItemStack item = inventory.GetItem(slot);
			Game1.thisGame.spriteBatch.Draw(slotTexture, new Rectangle(CalculatePosition(), new Point(slotSize, slotSize)), Color.White);
			if (item != null)
			{
				Point pos = CalculatePosition();
				pos = new Point(pos.X + (slotSize - itemSize) / 2, pos.Y + (slotSize - itemSize) / 2);
				Rectangle target = new Rectangle(pos, new Point(itemSize, itemSize));
				item.Draw(gameTime, target, pos.ToVector2());
			}
			base.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{

		}
	}
}
