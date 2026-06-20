using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.World.Generator.Feature
{
    public abstract class BiomeFeature
    {
        public abstract void Decorate(BlockPos pos, World worldIn, Random random);
    }
}