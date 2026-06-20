using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Earthian.GameObject.GUI;
using Earthian.Utilities;


namespace Earthian.Runtime.Screens
{
	class MainMenuScreen : Screen
	{
		public enum State
		{
			Intro,
			WorldSelect
		}

		public static State MenuState;

		private Texture2D bg;
		private float n;
		private GUI guiIntro, guiWorldSelect;
		private Mouse mouse;

		public MainMenuScreen()
		{
			n = 0f;
		}

		public override void Init()
		{
			bg = Game1.thisGame.Content.Load<Texture2D>("Atmospheric/Textures/NGTGrad");
			guiIntro = new GUIMainMenu(Runtime.thisRuntime.manager);
			guiWorldSelect = new GUIWorldSelect(Runtime.thisRuntime.manager);
			mouse = new Mouse("Cursor/Cursor");
		}

		public override void PreDrawDirect(GameTime gameTime)
		{
			int r = 100;
			Point b = new Point(bg.Bounds.Width / 2 - r, bg.Bounds.Height / 2 - r);
			Point p = new Point((int)(Math.Sin(n) * r) + bg.Bounds.Width / 2 - b.X, (int)(Math.Cos(n) * r) + bg.Bounds.Height / 2 - b.Y);
			Drawing.NewBatch(false);
			Rectangle re = Utilities.ScreenUtils.Dimensions();
			Game1.thisGame.spriteBatch.Draw(bg, re, new Rectangle(p, b), Color.White);
			Drawing.EndBatch();
		}

		public override void PreDraw(Microsoft.Xna.Framework.GameTime gameTime)
		{

		}

		public override void PostDraw(Microsoft.Xna.Framework.GameTime gameTime)
		{

		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
		}

		public override void DrawDirect(Microsoft.Xna.Framework.GameTime gameTime)
		{
			Drawing.NewBatch(false);
			if (MenuState == State.Intro)
				guiIntro.Draw(gameTime);
			else
				guiWorldSelect.Draw(gameTime);
			mouse.Draw(gameTime);
			Drawing.EndBatch();
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			n = (n + 0.01f) % 360;
			if (MenuState == State.Intro)
				guiIntro.Update(gameTime);
			else
				guiWorldSelect.Update(gameTime);
			mouse.Update(gameTime, Microsoft.Xna.Framework.Input.Mouse.GetState());
		}
	}
}
