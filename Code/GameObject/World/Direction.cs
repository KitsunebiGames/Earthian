using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.World
{
    public class Direction 
    {
        public static readonly BlockPos NORTH = new BlockPos(0,1);
        public static readonly BlockPos SOUTH = new BlockPos(0, -1);
        public static readonly BlockPos EAST = new BlockPos(1, 0);
        public static readonly BlockPos WEST = new BlockPos(-1, 0);
        public static readonly BlockPos NORTHEAST = new BlockPos(1, 1);
        public static readonly BlockPos NORTHWEST = new BlockPos(-1, 1);
        public static readonly BlockPos SOUTHEAST = new BlockPos(1, -1);
        public static readonly BlockPos SOUTHWEST = new BlockPos(-1, -1);
        public static readonly BlockPos NONE = new BlockPos(0, 0);
    }
}
