using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.Atmosphere
{
    class IngameTime
    {


        #region Variables
        private Texture2D dayTexture, nightTexture, starmapTexture, moonTexture, sunTexture;
        private Vector2 pos, org;
        private float alpha1 = 255f, alpha2 = 255f;
        private bool isDay;
        private float currentTick = 0;

        public int Time;
        public int Days;
        public int Months;
        public int Years;
        #endregion


        #region Constructors
        public IngameTime()
        {
        }
        #endregion


        #region Init
        public void Init()
        {
            dayTexture = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/DAYGrad");
            nightTexture = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/NGTGrad");
            sunTexture = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/Sun");
            moonTexture = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/Moon");

            pos = new Vector2(0, Game1.thisGame.Window.ClientBounds.Height / 2);
            org = new Vector2(Game1.thisGame.Window.ClientBounds.Width / 2, Game1.thisGame.Window.ClientBounds.Height);
            isDay = true;
            alpha1 = 0;
            alpha2 = 510;
            //Vector2 orgMoon = new Vector2(Game1.thisGame.Window.ClientBounds.Width / 2, Game1.thisGame.Window.ClientBounds.Y - Game1.thisGame.Window.ClientBounds.Height / 4);
        }
        #endregion


        #region Functions


        #region XNA Runtime
        private float RotationAngle = 0;
        private Matrix rotMat = Matrix.Identity;
        
        public void Draw(GameTime gameTime)
        {
            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMinutes / 8;

            // TODO: Add your game logic here.
            RotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;



            pos = new Vector2(0, Game1.thisGame.Window.ClientBounds.Height / 2);
            org = new Vector2(Game1.thisGame.Window.ClientBounds.Width / 2, Game1.thisGame.Window.ClientBounds.Height + Game1.thisGame.Window.ClientBounds.Height / 4);


            //Nighttime skycolor (Goes to 24 ingame time)
            if (isDay)
            {                
                Game1.thisGame.spriteBatch.Draw(nightTexture, new Rectangle(0, 0, Game1.thisGame.Window.ClientBounds.Width, Game1.thisGame.Window.ClientBounds.Height), Color.White);
                Game1.thisGame.spriteBatch.Draw(dayTexture, new Rectangle(0, 0, Game1.thisGame.Window.ClientBounds.Width, Game1.thisGame.Window.ClientBounds.Height), new Color(255, 239, 255, (int)alpha1 / 2));
            }
            else
            {                
                Game1.thisGame.spriteBatch.Draw(nightTexture, new Rectangle(0, 0, Game1.thisGame.Window.ClientBounds.Width, Game1.thisGame.Window.ClientBounds.Height), new Color(255, 239, 255));
            }
            // Daytime skycolor (Goes to 12 ingame time)
            if (!isDay)
            {
                Game1.thisGame.spriteBatch.Draw(moonTexture, RotateAboutOrigin(pos, org, RotationAngle), null, new Color(Color.White, (int)alpha1 * 2), RotationAngle, new Vector2(sunTexture.Width / 2, sunTexture.Height / 2), 3f, SpriteEffects.None, 0);
            }
            if (isDay)
            {
                Game1.thisGame.spriteBatch.Draw(sunTexture, RotateAboutOrigin(pos, org, RotationAngle), null, new Color(Color.Yellow, (int)alpha1 * 2), RotationAngle, new Vector2(sunTexture.Width / 2, sunTexture.Height / 2), 3f, SpriteEffects.None, 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
            if (RotationAngle > 1.29)
            {
                alpha1 -= elapsed * 2;
                alpha2 -= elapsed * 2;
            }

            if (RotationAngle < 0.4)
            {
                alpha1 += elapsed * 2;
                alpha2 += elapsed * 2;
            }



            if (RotationAngle >= 1.7 && isDay)
            {
                RotationAngle = 0;
                isDay = false;
                //Console.WriteLine("Half a day has lasted, it was " + Time + " timeunits long, therefore 12 hours is " + gameTime.TotalGameTime.Minutes + " minutes, and " + gameTime.TotalGameTime.Seconds + " seconds long.");
            }
            else if (RotationAngle >= 1.7 && !isDay)
            {
                RotationAngle = 0;
                isDay = true;
                Days += 1;
                //Console.WriteLine(Days + " day(s) has past! it was " + Time + " Time units long. Therefore 12 hours is " + Time / 2 + ". the day/night cycle lasted for " + gameTime.TotalGameTime.Minutes + " minutes, and " + gameTime.TotalGameTime.Seconds + " seconds.");
                Time = 0;
            }

            Time += gameTime.ElapsedGameTime.Milliseconds;
        }
        #endregion


        #region Time Formatting
        public string GetTimeAsString()
        {
            return (((6 + GetTotalHour() / 60) % 24) + ":" + ((GetTotalMinute() / 60) % 60) + ":" + ((GetTotalSecond() / 60) % 60));
        }
        #endregion


        #region Time
        public float GetSecondFloat()
        {
            return ((GetTotalSecondFloat() / 60) % 60);
        }


        public int GetSecond()
        {
            return ((GetTotalSecond() / 60) % 60);
        }


        public float GetMinuteFloat()
        {
            return ((GetTotalMinuteFloat() / 60) % 60);
        }


        public int GetMinute()
        {
            return ((GetTotalMinute() / 60) % 60);
        }


        public float GetHourFloat()
        {
            return (6 + (GetTotalHourFloat() / 60) % 24);
        }


        public int GetHour()
        {
            return (6 + (GetTotalHour() / 60) % 24);
        }
        #endregion


        #region Total Time
        public float GetTotalSecondFloat()
        {
            return GetTotalMinuteFloat() * 60;
        }


        public int GetTotalSecond()
        {
            return (int)GetTotalMinuteFloat() * 60;
        }


        public float GetTotalMinuteFloat()
        {
            return GetTotalHourFloat() * 60;
        }


        public int GetTotalMinute()
        {
            return (int)GetTotalHourFloat() * 60;
        }


        public float GetTotalHourFloat()
        {
            return  6 + GetTotalHourBaseFloat() * 60;
        }


        public int GetTotalHour()
        {
            return 6 + (int)GetTotalHourBaseFloat() * 60 ;
        }

        #endregion


        #region Timebase
        private float GetTotalHourBaseFloat()
        {
            return (float)Time / 65288;
        }


        private int GetTotalHourBase()
        {
            return Time / 65288;
        }
        #endregion


        #region Helper Functions
        /// <summary>
        /// Rotates the sun and moon around the origin point. Additional rotation to the sprite is added in the draw code.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="origin"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }
        #endregion


        #endregion


    }
}
