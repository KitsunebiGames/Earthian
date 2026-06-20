using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.Utilities
{
    public class TileFace
    {
        /// <summary>
        /// A block that is suspended in air.
        /// </summary>
        public static TileTexturePosition AloneBlock = new TileTexturePosition(0, 0);

        public static TileTexturePosition FullBlock = new TileTexturePosition(0, 1);
        /// <summary>
        /// A block that is suspended in air, by a block holding it on the right side. (V)
        /// </summary>
        public static TileTexturePosition LeftAirborn = new TileTexturePosition(1, 0);
        /// <summary>
        /// A block that is suspended in air, by a block holding it on the left side. (H)
        /// </summary>
        public static TileTexturePosition RightAirborn = new TileTexturePosition(2, 0);
        /// <summary>
        /// The top of a pillar, that is not connected to any blocks by the side, or on top of it.
        /// </summary>
        public static TileTexturePosition TopPillar = new TileTexturePosition(3, 0);
        /// <summary>
        /// The bottom of a pillar, that is not connected to any blocks by the side, or on bottom of it.
        /// </summary>
        public static TileTexturePosition BottomPillar = new TileTexturePosition(3, 1);

        public static TileTexturePosition HorisontalPillar = new TileTexturePosition(1, 1);

        public static TileTexturePosition VerticalPillar = new TileTexturePosition(2, 1);

        public static TileTexturePosition BlockTopRight = new TileTexturePosition(0, 2);

        public static TileTexturePosition BlockTopLeft = new TileTexturePosition(1, 2);

        public static TileTexturePosition BlockBottomRight = new TileTexturePosition(0, 3);

        public static TileTexturePosition BlockBottomLeft = new TileTexturePosition(1, 3);

        public static TileTexturePosition BlockTop = new TileTexturePosition(2, 2);

        public static TileTexturePosition BlockBottom = new TileTexturePosition(3, 2);

        public static Dictionary<Byte, TileTexturePosition> Mappings = new Dictionary<Byte, TileTexturePosition> {
        {0,new TileTexturePosition(0,0)},//0000 UDLR
        {1,new TileTexturePosition(1,0)},//0001 R
        {2,new TileTexturePosition(2,0)},//0010 L
        {4,new TileTexturePosition(3,0)},//0100 D
        {15,new TileTexturePosition(0,1)},//1111 UDLR
        {3,new TileTexturePosition(1,1)},//0011 LR
        {12,new TileTexturePosition(2,1)},//1100 UD
        {8,new TileTexturePosition(3,1)},//1000 U
        {5,new TileTexturePosition(0,2)},//0101 DR
        {6,new TileTexturePosition(1,2)},//0110 DL
        {7,new TileTexturePosition(2,2)},//0111 DLR
        {11,new TileTexturePosition(3,2)},//1011 ULR
        {9,new TileTexturePosition(0,3)},//1001 UR
        {10,new TileTexturePosition(1,3)},//1010 UL
        {13,new TileTexturePosition(2,3)},//1101 UDR
        {14,new TileTexturePosition(3,3)},//1110 UDL
        };





    }

    public class TileTexturePosition
    {
        #region Variables
        private int x, y;
        #endregion


        #region Getters/Setters
        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        #endregion


        #region Constructors
        public TileTexturePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

    }


    class TileTextureAnimation
    {


        #region Variables
        private int frame;
        private TileTexturePosition size;
        private int animationLength;
        private TileTexturePosition[] pos;
        private float animationSpeed;
        #endregion


        #region Constructors
        public TileTextureAnimation(TileTexturePosition size, int animationLength, float animationSpeed, int startframe)
        {
            this.size = size;
            this.animationLength = animationLength;
            this.animationSpeed = animationSpeed;
            this.frame = startframe;
            int frameSet = 0;
            pos = new TileTexturePosition[animationLength];

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    pos[frameSet] = new TileTexturePosition(x, y);
                    if (frameSet >= animationLength)
                        break;
                }
            }
        }
        #endregion


        #region Functions
        public void NextFrame()
        {
            if (frame < animationLength)
                frame++;
            else
                frame = 0;
        }

        public void LastFrame()
        {
            if (frame != 0)
                frame--;
            else
                frame = animationLength - 1;
        }

        public TileTexturePosition GetFrameInfo()
        {
            return pos[frame];
        }
        #endregion


    }
}
