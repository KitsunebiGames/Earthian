using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.GUI
{
    public class GUIDebug : GUI
    {
        private World.World world;
        private Runtime.Screens.FPSCounter counter;
        private List<String> info;
        public static String debug = "";

        public GUIDebug(World.World world, Runtime.Screens.FPSCounter fps)
        {
            this.world = world;
            this.counter = fps;
            this.info = new List<String>();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            addMessage(debug);
            debug = "";
            SpriteFont font = Runtime.Runtime.thisRuntime.font;
            int height = (int)font.MeasureString("A").Y + 2;
            string msg;
            for (int i = 0; i < info.Count; i++)
            {
                msg = info[i];
                Game1.thisGame.spriteBatch.DrawString(font, msg, new Vector2(Game1.thisGame.Window.ClientBounds.Width - font.MeasureString(msg).X, height * i), Color.White);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            info.Clear();
        }

        public void addMessage(string message)
        {
            info.Add(message);
        }
    }
}
