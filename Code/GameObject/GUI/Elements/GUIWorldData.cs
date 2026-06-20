using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.World;
using Microsoft.Xna.Framework;

namespace Earthian.GameObject.GUI.Elements
{
    public class GUIWorldData : GUIFrame
    {
        private World.World world;
        private GUITextBox title;
        private GUITextBox desc;
        private GUITextBox dateTime;

        public GUIWorldData(World.World world)
            : base()
        {
            this.world = world;
            this.title = new GUITextBox("Nameless",-1,20);
            this.desc = new GUITextBox("No Description", -1, 20);
            this.dateTime = new GUITextBox("00/00/00 00:00:00", -1, 20);
            UpdateLayout();
            this.frameColor = Color.Green;
        }

        public void UpdateLayout()
        {
            this.title.SetPosition(0, 0);
            this.desc.SetPosition(0, 30);
            this.dateTime.SetPosition(0, 60);
        }
    }
}
