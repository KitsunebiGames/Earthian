using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Earthian.GameObject.Block.Blocks
{
    class BlockBasic : Block
    {
        public BlockBasic(int id, bool wall)
            : base(id, wall)
        {
        }

        public override void BlockUpdate(Microsoft.Xna.Framework.GameTime gameTime, World.World worldIn)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public static int Get_ID()
        {
            return Blocks.IdLookup(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
