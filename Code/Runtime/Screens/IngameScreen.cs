using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.World;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Earthian.GameObject.Block;
using Earthian.GameObject.Atmosphere;
using Earthian.GameObject.Entity;
using Earthian.GameObject.GUI;
using Earthian.GameObject.Item;
using System.Reflection;
using System.Threading;
using Earthian.Code.GameObject.GUI;
using Earthian.Utilities;

namespace Earthian.Runtime.Screens
{
    public class FPSCounter
    {
        public FPSCounter()
        {
        }

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
        }
    }

    class IngameScreen : Screen
    {
        public World world;
        public static int size = 8;
        public GUIManager guis;
        public GUIDebug debug;
        private int minfps, maxfps;
        private int currentwait, endwait = 40;
        private bool Debug = true;
        IngameTime sky;
        FPSCounter counter;
        public static IngameScreen thisScreen;

        public IngameScreen()
        {
            //EarthianProtocol.Net.ClientInterface c = new EarthianProtocol.Net.ClientInterface("127.0.0.1", 32767, null);
            //c.Listen();
            sky = new IngameTime();
            counter = new FPSCounter();
            thisScreen = this;
            this.world = new World();
        }

        public override void PreDrawDirect(GameTime gameTime)
        {
            Drawing.NewBatch(false);
            sky.Draw(gameTime);
            Drawing.EndBatch();
        }

        public override void Init()
        {
            guis = new GUIManager();
            world.Init();
            guis.AddGUI(new GameHUD((GameObject.Entity.Entities.EntityPlayer)this.world.entities[0]),"HUD");
            guis.AddGUI(new GUIInventory((GameObject.Entity.Entities.EntityPlayer)this.world.entities[0],this.world),"INVENTORY");
            guis.AddGUI(new GUISettingsMenu(),"SETTINGS");
			//guis[3] = new GUITestCrafting((GameObject.Entity.Entities.EntityPlayer)this.world.entities[0]);
            debug = new GUIDebug(this.world, this.counter);
            sky.Init();
			//((GUITestCrafting)guis[3]).world = world;
            this.world.entities[0].Init();
            minfps = 60;
            maxfps = 0;
        }

        public override void PreDraw(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public override void PostDraw(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            world.Draw(gameTime);
            counter.Update(gameTime);
        }

        public override void DrawDirect(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Drawing.NewBatch(false);
            guis.Draw(gameTime);
            if (Debug)
            {
                debug.addMessage("FPS: " + (int)counter.CurrentFramesPerSecond);
                debug.addMessage("AVG. FPS: " + (int)counter.AverageFramesPerSecond);
				debug.addMessage("X " + this.world.entities[0].posX);
				debug.addMessage("Y: " + this.world.entities[0].posY);
                debug.addMessage("TIME: " + sky.GetTimeAsString());
                debug.Draw(gameTime);
            }
            SpriteFont font = Runtime.thisRuntime.font;
            Game1.thisGame.spriteBatch.DrawString(font, "Earthian Development Build - Do not distribute.", new Vector2(4, Game1.thisGame.Window.ClientBounds.Height - font.MeasureString("A").Y), Color.White);
            Game1.thisGame.mouseCursor.Draw(gameTime);
            Drawing.EndBatch();
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            guis.Update(gameTime);
            debug.Update(gameTime);
            if (currentwait > endwait)
            {
                if (counter.CurrentFramesPerSecond < minfps)
                    minfps = (int)counter.CurrentFramesPerSecond;

                if (counter.CurrentFramesPerSecond > maxfps)
                    maxfps = (int)counter.CurrentFramesPerSecond;
            }
            currentwait++;
            world.Update(gameTime);
            sky.Update(gameTime);
        }
    }
}
