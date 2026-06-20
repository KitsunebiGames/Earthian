using System;
using Earthian.GameObject.Block.Blocks;

namespace Earthian
{
	class BlockWoodPlanks : BlockBasic
	{
		public BlockWoodPlanks(bool wall)
			: base(5, wall)
		{
			this.SetTextureName("BlockWoodPlanks");
			this.SetMaterial(BlockMaterial.WOOD);
			this.name = "Wooden Planks";
			this.textureSize = 8;
			this.isWall = wall;
		}
	}
}
