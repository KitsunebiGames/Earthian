using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject
{
    public class GameManager
    {
        public static GameManager thisGame;

        public GUI.GUIManager guiMgr;

        private string dir;
        private World.World currentWorld;

        public GameManager(string directory)
        {
            thisGame = this;
            this.dir = directory;
            this.guiMgr = new GUI.GUIManager();
        }

        public void SetWorld(World.World w)
        {
            this.currentWorld = w;
        }

        public World.World World { get { return this.currentWorld; } }
    }
}
