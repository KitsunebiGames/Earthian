using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Earthian.GameObject.GUI.Elements
{
    public abstract class GUIElement : GUI
    {
        private static int default_padding = 5; //how about a configuration setting for this

        private int posX = 0, posY = 0;
        private int sizeX = 0, sizeY = 0;

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }
        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public int SizeX
        {
            get { return sizeX; }
            set { sizeX = value; }
        }
        public int SizeY
        {
            get { return sizeY; }
            set { sizeY = value; }
        }

        protected bool locked = false;

        protected List<GUIElement> children;
        public GUIElement parent;

        public GUIElement()
        {
            this.children = new List<GUIElement>();
        }

        public void Add(GUIElement e)
        {
            children.Add(e);
            e.parent = this;
        }

        public GUIElement SetPosition(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
            return this;
        }

        public GUIElement SetSize(int sizeX, int sizeY)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            return this;
        }

        public GUIElement VerticalAlign()
        {
            int h = 0;
            for (int i = 0; i < children.Count; i++)
            {
                GUIElement c = children[i];
                c.UpdateSize();
                c.PosX = 0;
                c.PosY = h;
                h += c.SizeY;
            }
            return this;
        }

        public GUIElement HorizontalAlign()  //why would you use this
        {
            int w = 0;
            for (int i = 0; i < children.Count; i++)
            {
                GUIElement c = children[i];
                c.PosY = 0;
                c.PosX = w;
                w += c.SizeX;
            }
            return this;
        }

        public List<GUIElement> GetElements()
        {
            return children;
        }

        public GUIElement GetElement(int id)
        {
            return children[id];
        }

        public void Remove()
        {
            if (parent != null)
            {
                parent.children.Remove(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (hide) return;
            foreach (GUIElement e in this.children)
            {
                e.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GUIElement e in this.children)
            {
                e.Update(gameTime);
            }
        }

        public Point CalculatePosition()
        {
            if (parent == null)
            {
                return new Point(PosX, PosY);
            }
            else
            {
                Point p = parent.CalculatePosition();
                return new Point(p.X + PosX, p.Y + PosY);
            }
        }

        public bool GetClick()
        {
            return (GetMouseOver() && Utilities.Input.MouseClickOnce(Utilities.Input.MouseButtons.LEFT));
        }

        public bool GetMouseOver()
        {
            Rectangle b = GetBounds();
            Vector2 p = Utilities.Input.GetMousePos();
            return (p.X > b.X && p.X < b.X + b.Width && p.Y > b.Y && p.Y < b.Y + b.Height);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(CalculatePosition(), new Point(SizeX, SizeY));
        }

        public void UpdateSize()
        {
            if (locked) return;
            int h = 0;
            int w = 0;
            foreach (GUIElement e in this.children)
            {
                e.UpdateSize();
                if (e.PosY + e.SizeY > h) h = e.PosY + e.SizeY;
                if (e.PosX + e.SizeX > w) w = e.PosX + e.SizeX;
            }
            this.SizeY = h;
            this.SizeX = w;
        }
    }
}
