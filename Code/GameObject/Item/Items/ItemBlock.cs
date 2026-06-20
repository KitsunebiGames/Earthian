using Earthian.Runtime.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Item.Items
{
	public class ItemBlock : ItemConsumable
	{
		public ItemBlock(Block.Block block)
			: base(block.id + 32768)
		{
			this.texture = block.texture;
			this.texturePos = new Microsoft.Xna.Framework.Rectangle(0, 0, 8, 8);
			this.name = block.name;
			this.swingTime = 15;
            
		}

		public override bool Use(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv)
		{
			if (player.allowBlockPlace)
			{
				Block.Block b = Block.Blocks.Blocks.InstantiateBlock(this.id - 32768, false);
				if (player.PlaceBlock(b, world.GetMouseOver()))
				{
					b.InitWorld(world);
					if (!isFromInv)
						base.Use(player, world, stack, isFromInv);
				}
				return true;
			}
			return false;
		}

		public override bool AltUse(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv)
		{
			if (player.allowBlockPlace)
			{
				Block.Block b = Block.Blocks.Blocks.InstantiateBlock(this.id - 32768, true);
				if (player.PlaceBlock(b, world.GetMouseOver(), 1))
				{
					b.InitWorld(world);
					if (!isFromInv)
						base.Use(player, world, stack, isFromInv);
				}
				return true;
			}
			return false;
		}
	}
}
