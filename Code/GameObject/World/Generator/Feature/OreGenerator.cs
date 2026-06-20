using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.World.Generator.Feature
{
    public class OreGenerator : BiomeFeatureMulti
    {
        private Block.Block blockType;

        public OreGenerator(int count, Block.Block type)
            : base(count)
        {
            this.blockType = type;
        }

        public override void Decorate(BlockPos pos, World worldIn, Random random)
        {
            BlockPos newPos;
            for (int i = 0; i < count; i++)
            {
                newPos = pos.add(new BlockPos(random.Next(Chunk.ChunkSize),random.Next(Chunk.ChunkSize)));
                worldIn.SetBlock(newPos, blockType);
            }
        }
    }
}
