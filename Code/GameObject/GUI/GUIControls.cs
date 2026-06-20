using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.GUI.Elements;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Earthian.GameObject.Settings;

namespace Earthian.GameObject.GUI
{
	public class GUIControls : GUIScrollPanel
	{
		private int listeningBind = -1;
		private List<GUIButton> bindButtons;

		public GUIControls(int x, int y)
			: base(400, 300)
		{
			this.bindButtons = new List<GUIButton>();
			this.AddControlBindGUI();
			UpdateSize();
			this.locked = true;
		}

		public void AddControlBindGUI()
		{
			int r = 0;
			foreach (ControlBinds.Controls c in Enum.GetValues(typeof(ControlBinds.Controls)))
			{
				this.Add(new GUITextLabel(c.ToString().Replace("_", " ") + ":", 200, 40).SetPosition(0, r));
				GUIButton b = (GUIButton)new GUIButton(ControlBinds.GetBind(c).ToString(), 200, 40).SetPosition(200, r);
				b = b.SetCallback((GUIButton.CallBack)(() =>
					{
						this.ListenForBind((int)c);
						b.highlight = true;
					}));
				bindButtons.Add(b);
				this.Add(b);
				r += 40;
			}
		}

		public void ListenForBind(int bind)
		{
			if (listeningBind >= 0)
			{
				bindButtons[listeningBind].highlight = false;
			}
			this.listeningBind = bind;
		}

		public override void Update(GameTime gameTime)
		{
			if (hide)
				return;
			Keys[] k = Utilities.Input.GetPressedKeys();
			if (this.listeningBind >= 0 && k.Length > 0)
			{
				ControlBinds.SetBind(listeningBind, k[0]);
				bindButtons[listeningBind].highlight = false;
				bindButtons[listeningBind].SetText(k[0].ToString());
				listeningBind = -1;
			}
			foreach (GUIElement b in children)
			{
				b.Update(gameTime);
			}
			base.Update(gameTime);
		}
	}
}
