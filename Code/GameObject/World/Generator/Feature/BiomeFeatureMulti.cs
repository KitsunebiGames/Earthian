using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.World.Generator.Feature
{
    public abstract class BiomeFeatureMulti : BiomeFeature
    {
        public int count;

        public BiomeFeatureMulti(int count)
        {
            this.count = count;
        }
    }
}
