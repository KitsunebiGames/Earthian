using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Earthian.Utilities
{
    public class ScreenUtils
    {
        public static int ScreenX()
        {
            return Dimensions().Width;
        }

        public static int ScreenY()
        {
            return Dimensions().Height;
        }

        public static Rectangle Dimensions()
        {
			Rectangle r = Game1.thisGame.Window.ClientBounds;
			r.X = 0;
			r.Y = 0;
            return r;
        }

        public static Point Center()
        {
            return Dimensions().Center;
        }

        public static Point Center(Point dim)
        {
            Point c = Center();
            return new Point(c.X - dim.X / 2, c.Y - dim.Y / 2);
        }
    }
}
