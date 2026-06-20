using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.GUI.Elements;

namespace Earthian.GameObject.GUI
{
    public class GUISettingsMenu : GUITabbedWindow
    {
        public GUISettingsMenu()
            : base("Options", 1000, 1000)
        {
            AddTab(0, (GUIPanel)new GUIControls(0,GUIWindow.lipSize), "Controls");
            AddTab(1, (GUIPanel)new GUIVideoSettings(0, GUIWindow.lipSize), "Camera");
            this.SetPosition(300, 300);
            this.ShowTab(0);
        }
    }
}
