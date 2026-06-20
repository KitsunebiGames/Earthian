using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Item.Items
{
    public class ItemConsumable : ItemBase
    {
        public ItemConsumable(int id) : base(id, true) { }

        public override bool Use(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv)
        {
            stack.count -= 1;
            if (stack.count == 0) player.Inventory.Contents[player.heldItem] = null;
			return true; //base.Use(player, world, stack);
        }
    }
}
