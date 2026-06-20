using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.GUI.Elements
{
    public class GUIFrame : GUIElement
    {
        protected static Texture2D frameTexture;
        internal static Rectangle[] sources = {
            new Rectangle(0, 0, 8, 8), //TOPLEFT
            new Rectangle(8, 0, 16, 8), //TOP
            new Rectangle(24, 0, 8, 8), //TOPRIGHT
            new Rectangle(0, 8, 8, 16), //LEFT
            new Rectangle(0, 24, 8, 8), //BOTTOMLEFT
            new Rectangle(24, 8, 8, 16), //RIGHT
            new Rectangle(24, 24, 8, 8), //BOTTOMRIGHT
            new Rectangle(8, 24, 16, 8), //BOTTOM
                                       };

        protected Color frameColor;

        public GUIFrame()
        {
            frameColor = Color.Turquoise; //SETTING PLZ

            if (frameTexture == null) frameTexture = Game1.thisGame.Content.Load<Texture2D>("GUI/Border"); 
        }

        public override void Draw(GameTime gameTime)
        {
            if (hide) return;
            base.Draw(gameTime);
            int i = 0;
            foreach (Rectangle r in Targets())
            {
                Game1.thisGame.spriteBatch.Draw(frameTexture, r, sources[i], frameColor);
                i++;
            }
        }

        internal System.Collections.IEnumerable Targets()
        {
            Point dim = CalculatePosition();
            yield return new Rectangle(dim.X - 16, dim.Y - 16, 16, 16);
            yield return new Rectangle(dim.X, dim.Y - 16, SizeX, 16);
            yield return new Rectangle(dim.X + SizeX, dim.Y - 16, 16, 16);
            yield return new Rectangle(dim.X - 16, dim.Y, 16, SizeY);
            yield return new Rectangle(dim.X - 16, dim.Y + SizeY, 16, 16);
            yield return new Rectangle(dim.X + SizeX, dim.Y, 16, SizeY);
            yield return new Rectangle(dim.X + SizeX, dim.Y + SizeY, 16, 16);
            yield return new Rectangle(dim.X, dim.Y + SizeY, SizeX, 16);
        }
    }
}
