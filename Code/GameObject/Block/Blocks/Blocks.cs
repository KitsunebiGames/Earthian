using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.Block;

namespace Earthian.GameObject.Block.Blocks
{
	public enum BlockMaterial
	{
		NONE = 0,
		DIRT = 1,
		STONE = 2,
		WOOD = 3,
		PLANT = 4,
	}

	public enum BlockOreIDS
	{
		COPPER = 1,
		IRON = 2,
		TUNGSTEN = 3,
		BRASS = 4,
		COPPERZINC = 5,
		GALLIUM = 6,
		TIN = 7,
		LEAD = 8,
		BERYLLIUM = 9,
		MAGNESIUM = 10,
		SILVER = 11,
		GOLD = 12,
		PLATINUM = 13,
		AWESOMIUM = 14,
		MANDERLORIUM = 16,
		QAZINITE = 17,
		IMPERVIUM = 18,
		LUMINORIUM = 19,
		COBOLT = 20,
		MOJINIUM = 21,
		ALUMINIUM = 22,
		JUMONIUM = 23,
		LUMARIUM = 24,
		PLUTARIUM = 25,
		ZACONIUM = 26,
		KEKINITE = 27
	}

	public class BlockMaterials
	{
		public static int[] BASE_HARDNESS = { 0, 500, 2000, 1400, 800 };

		public static int GetBaseHardness(BlockMaterial material)
		{
			return BASE_HARDNESS[(int)material];
		}
	}

	public class Blocks
	{
		public static Dictionary<int, Type> BlockMappings = new Dictionary<int, Type>
		{
			{ 1, typeof(BlockStone) },
			{ 2, typeof(BlockCobblestone) },
			{ 3, typeof(BlockDirt) },
			{ 5, typeof(BlockWoodPlanks) },
			{ 666, typeof(BlockRedCubeThingy) }
		};

		public static Block InstantiateBlock(int id, bool wall)
		{
			Type t = Blocks.BlockMappings[id];
			return (Block)System.Activator.CreateInstance(t, new object[] { wall });
		}

		public static int IdLookup(Type t)
		{
			foreach (int key in BlockMappings.Keys)
			{
				if (BlockMappings[key] == t)
					return key;
			}
			return 0;
		}

	}
}
