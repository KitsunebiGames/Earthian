using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Block.Blocks
{
	class BlockStone : BlockBasic
	{
		public BlockStone(bool wall)
			: base(1, wall)
		{
			this.SetTextureName("cstone");
			this.SetMaterial(BlockMaterial.STONE);
			this.name = "Stone";
			this.textureSize = 16;
			this.isWall = wall;
		}
	}
}
