using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.GUI.Elements;
using Microsoft.Xna.Framework;
using Earthian.Runtime.Screens;

namespace Earthian.GameObject.GUI
{
	public class GUIMainMenu : GUIElement
	{
		private GUIButton buttonPlaySingle;
		private GUIButton buttonPlayMulti;
		private GUIButton buttonOptions;
		private GUIButton buttonExit;
		private GUIImage title;

		public GUIMainMenu(Runtime.ScreenManager mgr)
			: base()
		{
			buttonPlaySingle = new GUIButton("Singleplayer", 300, 40).SetCallback((GUIButton.CallBack)(() =>
				{
					MainMenuScreen.MenuState = MainMenuScreen.State.WorldSelect;
				})); //all three variables should be settable
			buttonPlayMulti = new GUIButton("Multiplayer", 300, 40).SetCallback((GUIButton.CallBack)(() =>
				{
					PlayGame(mgr);
				}));
			buttonExit = new GUIButton("Quit", 300, 40).SetCallback((GUIButton.CallBack)(() =>
				{
					Game1.thisGame.Exit();
				}));
			title = (GUIImage)new GUIImage("GUI/Logo").SetSize(400, 100);

			this.Add(buttonPlaySingle);
			this.Add(buttonPlayMulti);
			this.Add(buttonExit);
			this.Add(title);
		}

		public void UpdateLayout()
		{
			Point c = Utilities.ScreenUtils.Center();
			this.title.SetPosition(c.X - 200, 0);
			this.buttonPlaySingle.SetPosition(c.X - 150, c.Y - 50);
			this.buttonPlayMulti.SetPosition(c.X - 150, c.Y);
			this.buttonExit.SetPosition(c.X - 150, c.Y + 50);
		}

		public void PlayGame(Runtime.ScreenManager mgr)
		{
			mgr.PullScreen();
			mgr.PushScreen("ingame");
		}

		public override void Update(GameTime gameTime)
		{
			UpdateLayout();
			base.Update(gameTime);
		}
	}
}
