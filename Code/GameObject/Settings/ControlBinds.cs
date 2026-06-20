using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Earthian.GameObject.Settings
{
    public class ControlBinds
    {
        public enum Controls
        {
            Move_Right = 0,
            Move_Left = 1,
            Jump = 2,
            Inventory = 3,
            Drop_Item = 4
        }

        private static string filename = "Settings.conf";

        //protected static Keys[] keyBinds = new Keys[Enum.GetValues(typeof(Controls)).Length];
        internal static Keys[] keyBinds = new Keys[]
        {
            Keys.D,
            Keys.A,
            Keys.Space,
            Keys.Escape,
            Keys.Q,
        };

        static ControlBinds()
        {
            /*
            if (!System.IO.File.Exists(filename))
                System.IO.File.Create(filename);
            bool m = false;
            int control = 0; //setting to 0 so the dumb compiler is happy
            int bind;
            foreach (byte b in System.IO.File.ReadAllBytes(filename))
            {
                if (m)
                {
                    control = (int)b;
                }
                else
                {
                    bind = (int)b;
                    keyBinds[control] = (Keys)bind;
                }
                m = !m;
            }
             */
        }

        public static void SetBind(int bind, Keys key)
        {
            keyBinds[bind] = key;
        }

        public static Keys GetBind(Controls bind)
        {
            return keyBinds[(int)bind];
        }

        public static void SaveBinds()
        {
            //save all keys to a file
        }
    }
}
