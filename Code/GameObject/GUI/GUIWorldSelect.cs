using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.GUI.Elements;
using Microsoft.Xna.Framework;
using Earthian.Runtime.Screens;
using Earthian.GameObject.World;

namespace Earthian.GameObject.GUI
{
	public class GUIWorldSelect : GUIElement
	{
		private GUIButton buttonNewWorld;
		private GUIButton buttonBack;
		private GUIImage title;
		private GUITextBox screenTitle;
		private List<WorldFile> files;
		private GUIScrollPanel worldList;

		public GUIWorldSelect(Runtime.ScreenManager mgr)
			: base()
		{
			List<WorldFile> wF = WorldFile.GetWorlds();
			worldList = new GUIScrollPanel(500, 500);
			int index = 0;
			foreach (WorldFile f in wF)
			{
				Console.WriteLine("Found world: " + f.SubData.world_name + "...");
				worldList.Add(new GUIWorld(f.SubData.world_name, index));
				index++;
			}
			screenTitle = new GUITextBox("World Select", 300, 40);
			buttonNewWorld = new GUIButton("New World", 200, 40).SetCallback((GUIButton.CallBack)(() =>
				{
					PlayGame(mgr);
				})); //all three variables should be settable
			buttonBack = new GUIButton("Back", 200, 40).SetCallback((GUIButton.CallBack)(() =>
				{
					GoBack(mgr);
				}));
			title = (GUIImage)new GUIImage("GUI/Logo").SetSize(400, 100);

			this.Add(screenTitle);

			//this.Add(buttonPlayMulti);
			this.Add(worldList);
			this.Add(buttonBack);
			this.Add(buttonNewWorld);
			this.Add(title);
		}

		public void UpdateLayout(GameTime gameTime)
		{
			Point c = Utilities.ScreenUtils.Center();
			int b = Utilities.ScreenUtils.ScreenY();
			int s = Utilities.ScreenUtils.ScreenX();
			this.title.SetPosition(c.X - 200, 0);
			this.screenTitle.SetPosition(c.X - 150, 100);
			this.buttonNewWorld.SetPosition(c.X, b - 50);
			this.buttonBack.SetPosition(c.X - 200, b - 50);
			this.worldList.SetPosition(s / 4, 150);
			this.worldList.SetSize((s / 4) * 2 + 32, b - 120);
			foreach (GUIElement element in worldList.GetElements())
			{
				element.SetSize(worldList.SizeX - 32, element.SizeY);
				element.Update(gameTime);

			}
		}


		public void PlayGame(Runtime.ScreenManager mgr)
		{
			mgr.PullScreen();
			mgr.PushScreen("ingame");
		}


		public void GoBack(Runtime.ScreenManager mgr)
		{
			MainMenuScreen.MenuState = MainMenuScreen.State.Intro;
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			this.worldList.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			UpdateLayout(gameTime);
			base.Update(gameTime);
		}
	}
}
