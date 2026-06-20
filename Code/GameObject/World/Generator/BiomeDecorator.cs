using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.World.Generator.Feature;

namespace Earthian.GameObject.World.Generator
{
    public class BiomeDecorator
    {
        private List<BiomeFeature> biomeFeatures;

        public BiomeDecorator()
        {
            this.biomeFeatures = new List<BiomeFeature>();
        }

        public void Decorate(BlockPos pos, World worldIn, Random random)
        {
            foreach (BiomeFeature f in biomeFeatures)
            {
                f.Decorate(pos, worldIn, random);
            }
        }

        public void AddFeature(BiomeFeature f)
        {
            this.biomeFeatures.Add(f);
        }

        public void Clear()
        {
            this.biomeFeatures = new List<BiomeFeature>();
        }
    }
}
