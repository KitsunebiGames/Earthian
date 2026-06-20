using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Earthian.Utilities;

namespace Earthian.GameObject.GUI.Elements
{
    public class GUITabbedWindow : GUIWindow
    {
        public List<GUIPanel> panels;
        protected int activePanel = 0;

        public GUITabbedWindow(string title, int x, int y)
            : base(title, x, y)
        {
            panels = new List<GUIPanel>();
        }

        public void AddTab(int id, GUIPanel panel, string name)
        {
            GUIButton button = (GUIButton)new GUIButton(name, 100, GUIWindow.lipSize).SetCallback((GUIButton.CallBack)(() => { this.ShowTab(id); })).SetPosition(id * 100, 0);
            panels.Add(panel);
            panel.hide = true;
            this.main.Add(button);
        }

        public void ShowTab(int id)
        {
            foreach (GUIPanel p in panels)
            {
                p.hide = true;
                p.Remove();
            }
            activePanel = id;
            panels[activePanel].hide = false;
            this.Add(panels[activePanel]);
            VerticalAlign();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GUIElement e in main.GetElements())
            {
                e.Update(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
