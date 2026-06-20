using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian
{
    public class Mouse
    {
        bool doSpin;
        bool isInWindow;
        bool wasInWindow;
        Texture2D cursor;
        MouseState mouse;
        Rectangle lastMouse;
        Rectangle size;
        Rectangle position;


        public Mouse(string mouseTexture)
        {
            cursor = Game1.thisGame.Content.Load<Texture2D>(mouseTexture);
            doSpin = false;
            size = new Rectangle(32, 32, 0, 0);
        }

        public void setSize(int size)
        {
            this.size = new Rectangle(size, size, 0, 0);
        }

        private float RotationAngle;
        public void Update(GameTime gameTime, MouseState mouseState)
        {
            mouse = mouseState;

            if (mouse.X < Game1.thisGame.Window.ClientBounds.Width
                && mouse.X > Game1.thisGame.Window.ClientBounds.X
                && mouse.Y < Game1.thisGame.Window.ClientBounds.Height
                && mouse.Y > Game1.thisGame.Window.ClientBounds.Y)
                isInWindow = true;
            else isInWindow = false;



            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.P) && Keyboard.GetState().IsKeyDown(Keys.I) && Keyboard.GetState().IsKeyDown(Keys.N))
            {
                doSpin = true;
            }
            else
                doSpin = false;



            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            RotationAngle += elapsed * 8;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;


                position.X = mouse.X;
                position.Y = mouse.Y;
                lastMouse = new Rectangle(mouse.X, mouse.Y, 0, 0);

            /*else if (!isInWindow && wasInWindow)
            {
                if (mouse.Y < Game1.thisGame.Window.ClientBounds.Y)
                    position.Y = Game1.thisGame.Window.ClientBounds.Y;
                if (mouse.X < Game1.thisGame.Window.ClientBounds.X)
                    position.Y = Game1.thisGame.Window.ClientBounds.X;
            }*/
            wasInWindow = isInWindow;
        }

        public void Draw(GameTime gameTime)
        {
            if (doSpin)
            {
                Game1.thisGame.spriteBatch.Draw(cursor, new Rectangle(position.X, position.Y, size.X, size.Y), null, new Color(0, 1, 0, 0.7f), RotationAngle, new Vector2(16 / 2, 16 / 2), SpriteEffects.None, 1);
            }
            if (!doSpin)
            {
                Game1.thisGame.spriteBatch.Draw(cursor, new Rectangle(position.X, position.Y, size.X, size.Y), new Color(0, 1, 0, 0.7f));
            }
        }
    }
}
