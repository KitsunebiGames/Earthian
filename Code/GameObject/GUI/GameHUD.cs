using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Earthian.Utilities;
using Earthian.GameObject.GUI.Elements;

namespace Earthian.GameObject.GUI
{
	public class GameHUD : GUIElement
	{
		private Texture2D HotbarWidgets, ManaFill, HealthFill;
		private Entity.Entities.EntityPlayer player;

		private Rectangle HealthBarFrame = new Rectangle(0, 0, 128, 26);
		private Rectangle ManaBarFrame = new Rectangle(0, 26, 128, 26);
		private Effect barDecay1, barDecay2;

		private Camera mCamera;

		private float hpfill, mpfill;

		public GameHUD(Entity.Entities.EntityPlayer player)
			: base()
		{
			this.player = player;
			HotbarWidgets = Game1.thisGame.Content.Load<Texture2D>("GUI/GUIStatusBarHD3");
			ManaFill = Game1.thisGame.Content.Load<Texture2D>("GUI/GUIManaFill");
			HealthFill = Game1.thisGame.Content.Load<Texture2D>("GUI/GUIHealthFill");
			hpfill = 1;
			mpfill = 0;
			mCamera = new Camera(Game1.thisGame.GraphicsDevice.Viewport);
			Game1.thisGame.AddCamera(mCamera);
			mCamera.Zoom = 5.0f;
			mCamera.centerPos = new Vector2(mCamera.centerPos.X, 0);
			//barDecay1 = Game1.thisGame.Content.Load<Effect>("");
		}

		public void DrawBars(GameTime gameTime)
		{
			int sw = Game1.thisGame.Window.ClientBounds.Width;
			int scale = (int)(sw / 400.0f);
			mCamera.Zoom = scale;

			Game1.thisGame.spriteBatch.End();
			Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, mCamera.Transform);


			//Rectangle healthBar = new Rectangle(sw / 2 - 128 * scale, 10, 128 * scale, 26 * scale);
			//Rectangle manaBar = new Rectangle(sw / 2, 10, 128 * scale, 26 * scale);

			Rectangle healthBar = new Rectangle(-128, 10, 128, 26);
			Rectangle manaBar = new Rectangle(0, 10, 128, 26);

			Rectangle healthBarSegment = new Rectangle(0, 0, 128, 7);
			Rectangle manaBarSegment = new Rectangle(0, 0, 128, 7);

			//Rectangle healthBarFill = new Rectangle(sw / 2 - (int)(128 * scale), 21, (int)(127 * scale), 7 * scale);
			//Rectangle manaBarFill = new Rectangle(sw / 2 + 16, 21, (int)(127 * scale), 7 * scale);

			Rectangle healthBarFill = new Rectangle(healthBar.X + 29, healthBar.Y + 17, 93, 7);

			Rectangle manaBarFill = new Rectangle(manaBar.X + 6, manaBar.Y + 17, 93, 7);
			Game1.thisGame.spriteBatch.Draw(HealthFill, healthBarFill, healthBarSegment, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
			Game1.thisGame.spriteBatch.Draw(ManaFill, manaBarFill, manaBarSegment, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);


			Game1.thisGame.spriteBatch.End();
			Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, mCamera.Transform);

           
			Game1.thisGame.spriteBatch.Draw(HotbarWidgets, healthBar, HealthBarFrame, Color.White);
			Game1.thisGame.spriteBatch.Draw(HotbarWidgets, manaBar, ManaBarFrame, Color.White);
			if (Utilities.Input.GetMouseOver(healthBar))
			{
				SpriteFont font = Game1.thisGame.Content.Load<SpriteFont>("Fonts/MainFont");
				string text = String.Format("{0}/{1}", player.Health, player.MaxHealth);
				Game1.thisGame.spriteBatch.DrawString(font, text, new Vector2((healthBar.X + healthBar.Width / 2) - (font.MeasureString(text).X / 2), (healthBar.Height) - font.MeasureString(text).Y / 2), Color.White);
			}
			Game1.thisGame.spriteBatch.End();
			Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
		}

		public override void Draw(GameTime gameTime)
		{
			this.DrawBars(gameTime);
		}

		public int GetProcentAsInt(float highestNum, float value)
		{
			return 0; 
		}

		public override void Update(GameTime gameTime)
		{
			mCamera.Update();
			mCamera.Pos = new Vector2(0, 0 + ((Game1.thisGame.Window.ClientBounds.Height / 2 + 16) / mCamera.Zoom));
			hpfill = (float)player.Health / (float)player.MaxHealth;
			mpfill -= 0.001f;
			if (mpfill <= 0f)
				mpfill = 1f;
		}
	}
}
