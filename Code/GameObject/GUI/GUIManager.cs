using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Earthian.GameObject.GUI.Elements;

namespace Earthian.GameObject.GUI
{
    public class GUIManager
    {
        private Dictionary<string, GUI> guis;

        public GUIManager()
        {
            this.guis = new Dictionary<string, GUI>();
        }

        public void AddGUI(GUI gui, string name)
        {
            this.guis.Add(name, gui);
        }

        public bool IsOpen(string name)
        {
            if (this.guis.Keys.Contains(name))
            {
                return (!this.guis[name].hide);
            }
            return false;
        }

        public void HideGUI(string name)
        {
            if (this.guis.Keys.Contains(name))
            {
                this.guis[name].hide = true;
            }
        }

        public void ShowGUI(string name)
        {
            if (this.guis.Keys.Contains(name))
            {
                this.guis[name].hide = false;
            }
        }

        public void ToggleGUI(string name)
        {
            if (this.guis.Keys.Contains(name))
            {
                this.guis[name].hide = !this.guis[name].hide;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (GUI g in this.guis.Values)
            {
                g.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            //start spritebatch
            foreach (GUI g in this.guis.Values)
            {
                g.Draw(gameTime);
            }
            //finish spritebatch
        }

        public bool IsInGUI()
        {
            foreach (GUI g in this.guis.Values)
            {
                if (Utilities.Input.GetMouseOver(((GUIElement)g).GetBounds()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
