using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.GUI.Elements
{
	public class GUITextLabel : GUIPanel
	{
		private string text;
		private Color textColor;

		public bool Centered = false;

		public GUITextLabel(string s, int x, int y)
			: base(x, y)
		{
			locked = true;
			this.text = s;
			this.textColor = Color.White; //setting for this plz
		}

		public void SetText(string s)
		{
			this.text = s;
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Draw(gameTime);
			SpriteFont f = Runtime.Runtime.thisRuntime.font;
			Vector2 pos = GetBounds().Center.ToVector2();
			if (Centered)
				pos = new Vector2(pos.X - f.MeasureString(text).X / 2, pos.Y - f.MeasureString("A").Y / 2);
			else
				pos = new Vector2(pos.X, pos.Y - f.MeasureString("A").Y / 2);
			Game1.thisGame.spriteBatch.DrawString(f, text, pos, textColor);
		}
	}
}
