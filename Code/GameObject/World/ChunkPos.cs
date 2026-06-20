using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.World
{
    public class ChunkPos
    {


        #region Variables
        protected int x;
        protected int y;
        #endregion


        #region Constructor
        public ChunkPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion


        #region Getters/Setters
        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }
        #endregion


        #region Functions
        public int[] ToIntPair()
        {
            return new int[2] { this.x, this.y };
        }

        public ChunkPos add(ChunkPos other)
        {
            return new ChunkPos(this.x + other.x, this.y + other.y);
        }

        public BlockPos ToBlockPos(bool direct = false)
        {
            int m = direct ? 1 : Chunk.ChunkSize;
            return new BlockPos(this.x * m, this.y * m);
        }

        public System.Collections.IEnumerable GetAdjacentChunks(int radius)
        {
            for (int x = -radius; x < radius + 1; x++)
            {
                for (int y = -radius; y < radius + 1; y++)
                {
                    yield return new ChunkPos(this.x + x, this.y + y);
                }
            }
        }

        public string Repr()
        {
            return String.Format("[{0},{1}]", this.X, this.Y);
        }

        public System.Collections.IEnumerable GetAdjacentChunks(int radiusX, int radiusY)
        {
            for (int x = -radiusX - 1; x < radiusX + 1; x++)
            {
                for (int y = -radiusY - 1; y < radiusY + 1; y++)
                {
                    yield return new ChunkPos(this.x + x, this.y + y);
                }
            }
            yield return this;
        }
        #endregion


    }
}
