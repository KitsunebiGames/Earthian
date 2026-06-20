using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.GUI.Elements
{
    public class GUISlider : GUIElement
    {
        protected static Texture2D sliderTexture;

        protected Color barColor = Color.Teal; //setting
        private int sliderPos = 0;
        private int snap = -1;

        public GUISlider(float position, int sX, int sY)
        {
            SizeX = sX;
            SizeY = sY;
            locked = true;
            SetSliderPosition(position);

            if (sliderTexture == null) sliderTexture = Game1.thisGame.Content.Load<Texture2D>("GUI/Button"); 
        }

        public void SetSliderPosition(float pos)
        {
            sliderPos = (int)(SizeX * pos);
        }

        public float GetSliderPosition()
        {
            return (float)sliderPos / (float)SizeX;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //needs redo -- now it's redone, and still looks crap
            base.Draw(gameTime);
            Point pos = CalculatePosition();
            //Left end
            Rectangle dest = new Rectangle(pos,new Point(SizeY, SizeY));
            Rectangle src = new Rectangle(24, 0, 8, 16);
            Game1.thisGame.spriteBatch.Draw(sliderTexture, dest, src, barColor);
            //Middle
            dest = new Rectangle(pos.X + SizeY, pos.Y, SizeX - SizeY*2, SizeY);
            src = new Rectangle(8, 0, 16, 16);
            Game1.thisGame.spriteBatch.Draw(sliderTexture, dest, src, barColor);
            //Right end
            dest = new Rectangle(pos.X + SizeX - SizeY, pos.Y, SizeY, SizeY);
            src = new Rectangle(32, 0, 8, 16);
            Game1.thisGame.spriteBatch.Draw(sliderTexture, dest, src, barColor);
            //Slider grip
            dest = new Rectangle(pos.X + sliderPos - SizeY/2, pos.Y, SizeY/2, SizeY);
            src = new Rectangle(0, 0, 8, 16);
            Game1.thisGame.spriteBatch.Draw(sliderTexture, dest, src, Color.White);
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (GetMouseOver() && Utilities.Input.GetMouseClick(Utilities.Input.MouseButtons.LEFT))
            {
                sliderPos = (int)(Utilities.Input.GetMousePos().X - CalculatePosition().X);
            }
        }
    }
}
