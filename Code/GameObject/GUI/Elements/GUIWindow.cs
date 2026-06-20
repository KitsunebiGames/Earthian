using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Earthian.Utilities;

namespace Earthian.GameObject.GUI.Elements
{
	public class GUIWindow : GUIFrame
	{
		protected static int lipSize = 32;

		protected string title;
		private bool Dragging;
		private Vector2 oldMousePos;
		protected bool Locked;

		public GUIPanel main;
		public GUITextLabel top;

		public GUIWindow(string title, int x, int y)
		{
			this.title = title;
			Dragging = false;
			Locked = false;
			top = (GUITextLabel)(new GUITextLabel(title, x, lipSize));
			main = (GUIPanel)(new GUIPanel(x, y));
			this.Add(top);
			this.Add(main);
			this.VerticalAlign();
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			this.top.SizeX = this.main.SizeX;
			UpdateSize();
			if (!Dragging && top.GetClick() && !Locked)
			{
				Dragging = true;
				oldMousePos = Input.GetMousePos();
			}
			else if (!Input.GetMouseClick(Input.MouseButtons.LEFT))
			{
				Dragging = false;
			}

			if (Dragging)
			{
				Vector2 newMousePos = Input.GetMousePos();
				PosX += (int)(newMousePos.X - oldMousePos.X);
				PosY += (int)(newMousePos.Y - oldMousePos.Y);
				oldMousePos = newMousePos;
			}
		}
	}
}
