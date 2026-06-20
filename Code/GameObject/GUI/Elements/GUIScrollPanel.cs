using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.GUI.Elements
{
    public class GUIScrollPanel : GUIPanel
    {
        private int scrollPos = 0;
        private int scrollMax = 0;

        public GUIScrollPanel(int sizeX, int sizeY)
            : base(sizeX, sizeY)
        {

        }

        public GUIScrollPanel SetScrollMax(int n)
        {
            this.scrollMax = n;
            return this;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Rectangle hitbox = GetBounds();
            hitbox.Inflate(10, 10);
            DrawPanel(gameTime);
            PosY -= scrollPos;
            foreach (GUIElement e in children)
            {
                if (hitbox.Contains(e.GetBounds()))
                {
                    e.Draw(gameTime);
                }
            }
            PosY += scrollPos;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            scrollPos -= Utilities.Input.GetMouseScroll();
            scrollPos = MathHelper.Clamp(scrollPos, 0, scrollMax);
        }

        public new void UpdateSize()
        {
            int oldSize = SizeY;
            base.UpdateSize();
            SetScrollMax(SizeY - oldSize);
            SizeY = oldSize;
        }

        /*
        public new Point CalculatePosition()
        {
            Point p = base.CalculatePosition();
            return new Point(p.X, p.Y - scrollPos);
        }
         */
    }
}
