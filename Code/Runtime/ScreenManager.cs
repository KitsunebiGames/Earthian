using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.Runtime.Screens;

namespace Earthian.Runtime
{


	#region ScreenManager
	public class ScreenManager
	{
		Dictionary<string, Screen> screens;
		private string currentScreen;
		private string lastScreen;

		public ScreenManager()
		{
			screens = new Dictionary<string, Screen>();
			currentScreen = "";
			lastScreen = "";
		}

		public void AddScreen(string screenRefName, Screen data)
		{
			screens.Add(screenRefName, data);
		}

		public void PushScreen(string screenName)
		{
			lastScreen = currentScreen;
			currentScreen = screenName;
		}

		public void PullScreen()
		{
			string tmp;
			tmp = currentScreen;
			currentScreen = lastScreen;
			lastScreen = tmp;
		}


		public void Update(GameTime gameTime)
		{
			screens[currentScreen].Update(gameTime);
		}

		#region Drawing

		public void PreDraw(GameTime gameTime)
		{
			screens[currentScreen].PreDraw(gameTime);
		}

		public void PreDrawDirect(GameTime gameTime)
		{
			screens[currentScreen].PreDrawDirect(gameTime);
		}

		public void Draw(GameTime gameTime)
		{
			screens[currentScreen].Draw(gameTime);
		}

		public void DrawDirect(GameTime gameTime)
		{
			screens[currentScreen].DrawDirect(gameTime);
		}

		public void PostDraw(GameTime gameTime)
		{
			screens[currentScreen].PostDraw(gameTime);
		}

		#endregion


		public void Init()
		{
			/*
            if (screens[lastIndexUpdate].isSilentAlive)
                screens[lastIndexUpdate].Init();

            screens[currentIndexUpdate].Init();
             */
			foreach (Screen s in screens.Values)
			{
				if (s != null)
					s.Init();
			}
		}
	}
	#endregion


}
