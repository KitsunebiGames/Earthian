using System;
using IronPython.Runtime;

namespace Earthian.GameObject.GUI.Elements
{
	public class GUIWorld : GUIElement
	{
		private GUIPanel panel;
		private GUIButton butt;
		private GUITextLabel lab;
		private int Index;

		public GUIWorld(string worldname, int index)
			: base()
		{
			//this.SizeX = 512;
			//this.SizeY = 128;
			this.PosY = (this.SizeY * index) + 2;
			this.Index = index;
			panel = new GUIPanel(this.SizeX, 64);
			lab = new GUITextLabel("World: " + worldname, 32, 32);
			lab.Centered = false;
			butt = new GUIButton("Load", 128, 40).SetCallback((GUIButton.CallBack)(() =>
				{
					
				}));
			panel.Add(lab);
			panel.Add(butt);
			this.Add(panel);
			//this.Add(butt);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			this.PosY = (this.SizeY * Index) + 2;

			butt.SetPosition(this.SizeX - butt.SizeX, 32);
			butt.Update(gameTime);
			base.Update(gameTime);
			//this.SizeX = 512;
			//this.SizeY = 128;

			//panel.SetSize(parent.SizeX, 128);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
			
			base.Draw(gameTime);
		}
	}
}

