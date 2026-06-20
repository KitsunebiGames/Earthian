using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Code will mostly be identical to TileTexture.cs, though this is just to organise stuffs. Changes will be made later.
/// TODO: Add descriptive info about functions, etc.
/// </summary>
namespace Earthian.Utilities
{
        public class EntityTextureAnimation
        {
            #region Variables
            private int frame;
            private Texture2D texture;
            private EntityTexturePosition size;
            private int animationLength;
            private EntityTexturePosition[] pos;
            private int animationSpeed;
            private int frameTime;
            #endregion

            #region Getters/Setters
            public int Frame
            {
                get { return frame; }
                set { frame = value; }
            }
            public int Length
            {
                get { return animationLength - 1; }
            }
            public Rectangle GetFramePosition()
            {
                return new Rectangle(frame * size.X, 0, size.X, size.Y);
            }
            #endregion

            #region Constructors
            public EntityTextureAnimation(EntityTexturePosition size, int animationLength, int animationSpeed, int startframe, Texture2D texture)
            {
                this.size = size;
                this.texture = texture;
                this.animationLength = animationLength;
                this.animationSpeed = animationSpeed;
                this.frame = startframe;
                this.frameTime = 0;
                int frameSet = 0;
                pos = new EntityTexturePosition[animationLength];

                for (int x = 0; x < animationLength/2; x++)
                {
                    for (int y = 0; y < animationLength/2; y++)
                    {
                        pos[frameSet] = new EntityTexturePosition(x, y);
                        if (frameSet >= animationLength - 1)
                            break;
                        frameSet++;
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

            public void UpdateFrame(int time)
            {
                frameTime += time;
                if (frameTime >= animationSpeed) { NextFrame(); frameTime %= animationSpeed; }
            }

            public EntityTexturePosition GetFrameInfo()
            {
                return pos[frame];
            }

            public void Draw(GameTime time, Rectangle pos, Boolean flip)
            {
                Rectangle position = GetFramePosition();
                if (!flip)
                    Game1.thisGame.spriteBatch.Draw(texture, pos, new Rectangle(this.Frame * position.Width, 0, position.Width, position.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                else
                    Game1.thisGame.spriteBatch.Draw(texture, pos, new Rectangle(this.Frame * position.Width, 0, position.Width, position.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            #endregion
        }
        public class EntityTexturePosition
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
            public EntityTexturePosition(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            #endregion
        }

    #region Multi Size
        /// <summary>
        /// A position in a spritesheet for the animation of EntityTextureAnimation.
        /// </summary>
        public class EntityMultiSizeTexturePosition
        {


            #region Variables
            EntityTexturePosition[] position, size;
            
            #endregion


            #region Getters/Setters
            public EntityTexturePosition[] Sizes
            {
                get { return size; }
            }

            public EntityTexturePosition[] Positions
            {
                get { return position; }
            }

            #endregion


            #region Constructors
            public EntityMultiSizeTexturePosition(EntityTexturePosition[] position, EntityTexturePosition[] size)
            {
                this.position = position;
                this.size = size;
            }
            #endregion


            #region Functions
            public EntityTexturePosition getPosition(int index)
            {
                return position[index];
            }

            public EntityTexturePosition getSize(int index)
            {
                return size[index];
            }

            #endregion

        }

    /*
        public class EntityMultiSizeTextureAnimation
        {


            #region Variables
            private int frame;
            private EntityMultiSizeTexturePosition textures;
            private int animationLength;
            private float animationSpeed;
            #endregion


            #region Constructors
            public EntityMultiSizeTextureAnimation(EntityMultiSizeTexturePosition texturePositions, int animationLength, float animationSpeed, int startframe)
            {
                this.textures = textures;
                this.animationLength = animationLength;
                this.animationSpeed = animationSpeed;
                this.frame = startframe;
                int frameSet = 0;

                for (int index = 0; index < texturePositions.Positions.Length; index++)
                {

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

            public EntityTexturePosition GetFrameSizeInfo()
            {
                return textures.Sizes[frame];
            }

            public EntityTexturePosition GetFramePositionInfo()
            {
                return textures.Positions[frame];
            }
            #endregion


        }
     * */
    #endregion
    public class EntityAnimationHandler
    {
        private int state;

        private EntityTextureAnimation[] anims;

        public EntityAnimationHandler(int c)
        {
            this.anims = new EntityTextureAnimation[c];
        }

        public void changeState(int s, Boolean resetFrame)
        {
            if (s == state) return;
            state = s;
            if (resetFrame) anims[s].Frame = 0;
        }

        public void Step()
        {
            anims[state].NextFrame();
        }

        public void addAnim(int state, Texture2D tex, int frames, EntityTexturePosition size, int speed)
        {
            this.anims[state] = new EntityTextureAnimation(size,frames,speed,0,tex);
        }

        public void Draw(GameTime time, Rectangle pos, Boolean flip)
        {
            this.anims[state].Draw(time,pos,flip);
        }

        public void Update(GameTime time)
        {
            anims[state].UpdateFrame(time.ElapsedGameTime.Milliseconds);
        }
    }


}
