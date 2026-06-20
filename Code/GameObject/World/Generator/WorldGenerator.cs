using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.World;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.World.Generator
{
    public abstract class WorldGenerator
    {


        #region Variables
        private string seed;
        #endregion


        #region Constructor
        public WorldGenerator(string seed)
        {
            this.seed = seed;
        }
        #endregion


        #region Functions
        /// <summary>
        /// Generates a chunk from the specified chunk position.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public abstract Chunk GenerateChunk(ChunkPos pos);
        #endregion


    }
}
