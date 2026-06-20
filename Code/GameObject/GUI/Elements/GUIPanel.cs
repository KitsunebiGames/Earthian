using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.GUI.Elements
{
	public class GUIPanel : GUIElement
	{
		protected static Texture2D panelTexture;

		protected Color color;

		public GUIPanel(int sizeX, int sizeY)
		{
			this.SizeX = sizeX;
			this.SizeY = sizeY;
			this.color = Color.Turquoise; //default color can be set in settings plz? :3 -- sure - Theis

			if (panelTexture == null)
				panelTexture = Game1.thisGame.Content.Load<Texture2D>("GUI/GUIPanel"); 
		}

		public override void Draw(GameTime gameTime)
		{
			if (!hide)
			{
				DrawPanel(gameTime);
				base.Draw(gameTime);
			}
		}

		protected void DrawPanel(GameTime gameTime)
		{
			Game1.thisGame.spriteBatch.Draw(panelTexture, new Rectangle(CalculatePosition(), new Point(SizeX, SizeY)), color);
			//complicated later to look better
		}

		public override void Update(GameTime gameTime)
		{
			UpdateSize();
		}
	}
}
