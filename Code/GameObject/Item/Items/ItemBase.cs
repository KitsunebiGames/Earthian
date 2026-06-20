using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Item.Items
{
	public class ItemBase : Item
	{
		public ItemBase(int id)
			: base(id)
		{
            
		}

		public ItemBase(int id, bool consumable)
			: base(id, consumable)
		{

		}


		public override bool Use(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv)
		{
			return true;
		}

		public override bool AltUse(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv)
		{
			return true;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}
}
