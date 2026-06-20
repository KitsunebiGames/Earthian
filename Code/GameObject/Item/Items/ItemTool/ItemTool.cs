using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.Block.Blocks;
using Earthian.Utilities;

namespace Earthian.GameObject.Item.Items
{
	public class ToolSettings
	{
		private string name;
		private int[,] toolPowers;
		public Type type;
		public int ID;

		public ToolSettings(int id, string name, int[,] miningPower, Type type)
		{
			this.name = name;
			toolPowers = miningPower;
			this.type = type;
			this.ID = id;
		}

		public float GetMiningPower(BlockMaterial material)
		{
			for (int i = 0; i < toolPowers.Length; i++)
			{
				if (toolPowers[i, 0] == (int)material)
				{
					return (float)toolPowers[i, 1] / 1000;
				}
			}
			return 1f;
		}

		public string GetName()
		{
			return this.name;
		}
	}

	public class ItemTool : ItemBase
	{
		public enum EToolTier
		{
			//THESE ONES ARE PLACEHOLDERS
			Copper = 0,
			Iron = 1,
			Silver = 2,
			Gold = 3,
			Diamond = 4,
			Obsidian = 5,
		}

		public static readonly ToolSettings PICKAXE = new ToolSettings(0, "Pickaxe", new int[,] { { (int)BlockMaterial.STONE, 8000 }, { (int)BlockMaterial.DIRT, 3000 } }, typeof(ItemPickaxe));
		public static readonly ToolSettings SHOVEL = new ToolSettings(1, "Shovel", new int[,] { { (int)BlockMaterial.DIRT, 6000 } }, null);
		public static readonly ToolSettings AXE = new ToolSettings(2, "Axe", new int[,] { { (int)BlockMaterial.WOOD, 4000 } }, null);
		public static readonly ToolSettings SWORD = new ToolSettings(3, "Sword", new int[,] { { (int)BlockMaterial.PLANT, 2000 } }, null);

		private int toolTier;
		//used for certain blocks needing certain tiers of tool
		private int toolType;
		//type of tool this tool is

		public ItemTool(int id, int toolType, int toolTier)
			: base(id, false)
		{
			this.toolTier = toolTier;
			this.toolType = toolType;
			ToolSettings toolData = GetToolData();
			SetTextureName(String.Format("Items/Tools/{0}{1}", toolData.GetName(), toolTier));
			this.name = String.Format("{0} {1}", (EToolTier)toolTier, toolData.GetName());
			SetToolPower((toolTier + 1) * 50);
			SetSwingTime(15);
			SetScale(2f);
		}

		public ToolSettings GetToolData()
		{
			return GetToolData(this.toolType);
		}

		public new float GetToolPowerVsMaterial(Block.Blocks.BlockMaterial material)
		{
			ToolSettings toolPowers = GetToolData();
			return toolPowers.GetMiningPower(material);
		}

		public override bool Use(Entity.Entities.EntityPlayer player, World.World world, ItemStack stack, bool isFromInv)
		{
			if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				player.MineBlock(world.GetMouseOver(), 1, world);
			else
				player.MineBlock(world.GetMouseOver(), 0, world);
			return true; //base.Use(player, world, stack);
		}

		public ItemTool SetToolPower(int n)
		{
			this.toolPower = n;
			return this;
		}

		public ItemTool SetSwingTime(int n)
		{
			this.swingTime = n;
			return this;
		}


		public ItemTool SetScale(float n)
		{
			this.scale = n;
			return this;
		}

		public static System.Collections.IEnumerable GetToolTypes()
		{
			yield return PICKAXE;
			yield return SHOVEL;
			yield return AXE;
			yield return SWORD;
		}

		public static ToolSettings GetToolData(int toolType)
		{
			ToolSettings toolPowers = null;
			switch (toolType)
			{
				case 0:
					toolPowers = PICKAXE;
					break;
				case 1:
					toolPowers = SHOVEL;
					break;
				case 2:
					toolPowers = AXE;
					break;
				case 3:
					toolPowers = SWORD;
					break;
			}
			return toolPowers;
		}

		public new bool Compare(Item other)
		{
			return false;
		}
	}
}
