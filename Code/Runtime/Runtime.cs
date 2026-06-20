using Earthian.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.Runtime.Screens;

namespace Earthian.Runtime
{
	public class Messages
	{

		public static string[] messages = new string[]
		{
			"Who needs grass, when you have clouds?!",
			"Also try Terraria!",
			"Also try Minecraft!",
			"Also try Starbound!",
			"Flippety flop.",
			"This is a silly message",
			"Error 404: Error not found!",
			"The Game",
			"Moonbutt is best butt.",
			"Bad joke is bad.",
			"Warranty void if given warranty",
			"Almost as good as cake",
			"The HAAAAAAACCCKS!",
			"Fun for all the family!",
			"Not edible",
			"                                                                                           Ooops.",
			"What's the worst that could happen?",
			"Not actually made of dirt."
		};
	}

	public class Runtime
	{
		public SpriteFont font;
		public ScreenManager manager;
		public Camera mCamera;
		public static Runtime thisRuntime;

		public Runtime()
		{
			manager = new ScreenManager();
			manager.AddScreen("mainMenu", new MainMenuScreen());
			manager.AddScreen("ingame", new IngameScreen());
			manager.PushScreen("mainMenu");
			thisRuntime = this;
		}

		public void SetSillyWindowTitle()
		{
			Random random = new Random();
			Game1.thisGame.Window.Title = "Earthian: " + Messages.messages[random.Next(0, Messages.messages.Length)];
		}

		public void Update(GameTime gameTime)
		{
			manager.Update(gameTime);
			mCamera.Update();
		}

		public void Init()
		{
			Drawing.Init(); //loads shaders
			font = Game1.thisGame.Content.Load<SpriteFont>("Fonts/MainFont");
			mCamera = new Camera(Game1.thisGame.GraphicsDevice.Viewport);
			manager.Init();
			Game1.thisGame.AddCamera(mCamera);
			SetSillyWindowTitle();
		}

		public void SetCameraPos(Vector2 Pos)
		{
			mCamera.Pos = Pos;
		}

		public void ChangedViewport()
		{
			mCamera.ChangeViewport(Game1.thisGame.GraphicsDevice.Viewport);
		}

		public void PreDraw(GameTime gameTime)
		{
			manager.PreDraw(gameTime);
		}

		public void Draw(GameTime gameTime)
		{
			PreDraw(gameTime);
			PreDirectDraw(gameTime);
			manager.Draw(gameTime); //Draw(gameTime) except that is this method
			DrawDirect(gameTime);
			PostDraw(gameTime);
			//PostDirectDraw(gameTime); isn't used
		}

		public void PostDraw(GameTime gameTime)
		{
			//Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, mCamera.Transform);
			manager.PostDraw(gameTime);
			//Game1.thisGame.spriteBatch.End();
		}

		private void PreDirectDraw(GameTime gameTime)
		{
			//Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
			manager.PreDrawDirect(gameTime);
			//Game1.thisGame.spriteBatch.End();
		}

		private void DrawDirect(GameTime gameTime)
		{
			//Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
			manager.DrawDirect(gameTime);
			//Game1.thisGame.spriteBatch.End();
		}

		private void PostDirectDraw(GameTime gameTime)
		{
			//Game1.thisGame.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
			//manager.PostDrawDirect(gameTime);
			//Game1.thisGame.spriteBatch.End();
		}
	}
}
