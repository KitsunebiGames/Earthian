using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.GUI.Elements
{
    class GUIButton : GUIElement
    {
        public delegate void CallBack();

        private CallBack callBack;
        protected Texture2D buttonTexture;
        private string text;
        private Color textColor;
        public bool highlight = false;

        public GUIButton(string text, int x, int y)
        {
            textColor = Color.White;
            this.text = text;
            this.SizeX = x;
            this.SizeY = y;
            locked = true;

            if (buttonTexture == null) buttonTexture = Game1.thisGame.Content.Load<Texture2D>("GUI/Button"); 
        }

        public void SetText(string s)
        {
            this.text = s;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.thisGame.spriteBatch.Draw(buttonTexture, new Rectangle(CalculatePosition(), new Point(SizeX, SizeY)), textColor);
            SpriteFont f = Runtime.Runtime.thisRuntime.font;
            Vector2 pos = GetBounds().Center.ToVector2();
            pos = new Vector2(pos.X - f.MeasureString(text).X / 2, pos.Y - f.MeasureString("A").Y / 2);
            Game1.thisGame.spriteBatch.DrawString(f, text, pos, textColor);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (highlight) textColor = Color.Aqua; //setting
            else if (GetMouseOver())
            {
                this.textColor = Color.Green; //setting
            }
            else this.textColor = Color.White;
            if (callBack != null && this.GetClick()) callBack();
        }

        public GUIButton SetCallback(Delegate cb)
        {
            this.callBack = (CallBack)cb;
            return this;
        }
    }
}
