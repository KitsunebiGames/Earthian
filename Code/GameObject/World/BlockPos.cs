using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.World
{
    public class BlockPos : ChunkPos
    {


        #region Constructor
        public BlockPos(int x, int y) : base(x, y) { }
        #endregion


        #region Functions
        public BlockPos offset(BlockPos dir)
        {
            return this.add(dir);
        }

        public BlockPos add(BlockPos other)
        {
            return new BlockPos(this.x + other.x, this.y + other.y);
        }

        public ChunkPos ToChunkPos()
        {
            float c = (float)Chunk.ChunkSize;
            int nx = (int)Math.Floor(x / c);
            int ny = (int)Math.Floor(y / c);
            return new ChunkPos(nx,ny);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(this.X * 8, this.Y * 8);
        }

        public System.Collections.IEnumerable GetAdjacentSides(int radiusX, int radiusY)
        {
            for (int X = -radiusX; X < radiusX + 1; X++)
            {
                for (int Y = -radiusY; Y < radiusY + 1; Y++)
                {
                    yield return new BlockPos(this.x + X, this.y + Y);
                }
            }
            yield return this;
        }

        public static System.Collections.IEnumerable GetAdjacentSides(BlockPos position)
        {
            yield return position.offset(Direction.SOUTH);
            yield return position.offset(Direction.NORTH);
            yield return position.offset(Direction.WEST);
            yield return position.offset(Direction.EAST);
        }

        #endregion


    }
}
