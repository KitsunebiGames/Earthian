using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Earthian.GameObject.Item;
using Earthian.GameObject.GUI.Elements;
using Earthian.GameObject.Entity.Entities;
using Microsoft.Xna.Framework.Input;

namespace Earthian.GameObject.GUI
{
    public class GUIInventory : GUIWindow
    {
        private Entity.Entities.EntityPlayer player;
        public bool wasInventoryOpen;
        private ItemStack mheldItem;
        //private ItemStack heldItem;
        private World.World world;
        public Rectangle s;

        private int hotbarSize = 9;

        public GUIInventory(Entity.Entities.EntityPlayer player, World.World world)
            : base("Inventory", 0, 0)
        {
            this.player = player;
            this.world = world;
            int x;
            int y;
            for (int i = 0; i < player.Inventory.Contents.Length; i++)
            {
                x = (int)Math.Floor(i / (float)hotbarSize) * GUIItemSlot.slotSize;
                y = i % hotbarSize * GUIItemSlot.slotSize;
                this.main.Add(new GUIItemSlot(player.Inventory, i).SetPosition(x, y));
            }
            VerticalAlign();
            UpdateSize();
            hide = true;
        }

        public Rectangle GetBound()
        {
            return GetBounds();
        }

        public int GetSlotMouseOver()
        {
            for (int i = 0; i < main.GetElements().Count; i++)
            {
                GUIElement slot = main.GetElement(i);
                if (slot.GetMouseOver()) return i;
            }
            return -1;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (player.isInventoryOpen != wasInventoryOpen)
            {
                if (player.isInventoryOpen)
                    OpenInventory();
                else
                    CloseInventory();
            }
            if (Utilities.Input.IsControlPressedOnce(Settings.ControlBinds.Controls.Drop_Item))
            {
                if (player.Inventory.Contents[player.heldItem] != null)
                {
                    ThrowHeldItem(true, false);
                }
            }

            if (!hide)
            {
                //Click and drag items
                int slotOver = GetSlotMouseOver();
                if (slotOver >= 0)
                {
                    if (Utilities.Input.MouseClickOnce(Utilities.Input.MouseButtons.LEFT))
                    {
                        if (mheldItem == null)
                        {
                            mheldItem = player.Inventory.PopItem(slotOver);
                        }
                        else
                        {
                            mheldItem = player.Inventory.SetItem(mheldItem, slotOver);
                        }
                    }
                    if (Utilities.Input.MouseClickOnce(Utilities.Input.MouseButtons.RIGHT))
                    {
                        ItemStack splitStack = player.Inventory.GetItem(slotOver);
                        if (mheldItem == null && splitStack != null)
                        {
                            mheldItem = new ItemStack(splitStack.item, 1);
                            player.Inventory.Contents[slotOver].count -= 1;
                            if (player.Inventory.Contents[slotOver].count == 0) player.Inventory.Contents[slotOver] = null;
                        }
                        else if (splitStack != null)
                        {
                            mheldItem.count += 1;
                            player.Inventory.Contents[slotOver].count -= 1;
                            if (player.Inventory.Contents[slotOver].count == 0) player.Inventory.Contents[slotOver] = null;
                        }
                    }
                }
                if (slotOver == -1)
                {
                    if (Utilities.Input.MouseClickOnce(Utilities.Input.MouseButtons.RIGHT))
                    {
                        if (mheldItem != null)
                        {
                            ThrowHeldItem(false, false);
                        }
                    }
                    if (Utilities.Input.MouseClickOnce(Utilities.Input.MouseButtons.LEFT))
                    {
                        if (mheldItem != null)
                        {
                            if (mheldItem.item.Use(player, world, this.mheldItem, true))
                            {
                                if (mheldItem.item.Consumable)
                                {
                                    if (mheldItem.count > 1)
                                        this.mheldItem.count -= 1;
                                    else
                                        this.mheldItem = null;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (mheldItem != null)
                {
                    ThrowHeldItem(false, true);
                }
            }
            wasInventoryOpen = player.isInventoryOpen;
        }

        public void ThrowHeldItem(bool playerStack, bool getOne)
        {
            EntityItem i;
            if (!playerStack)
                i = new Entity.Entities.EntityItem(mheldItem);
            else
                if (getOne)
                    i = new Entity.Entities.EntityItem(new Item.ItemStack(this.player.GetHeldItem().item, 1));
                else
                    i = new Entity.Entities.EntityItem(this.player.GetHeldItem());

            if (player.doFlip)
                i.MotionX += 1;
            else
                i.MotionX -= 1;


            world.SpawnEntity(i, new Vector2(player.Position.X + 16, player.Position.Y));
            i.SetPickupTimer(6500);
            if (!playerStack)
                mheldItem = null;
            else
                if (player.Inventory.Contents[player.heldItem].count > 1)
                    player.Inventory.Contents[player.heldItem].count -= 1;
                else
                    player.Inventory.Contents[player.heldItem] = null;
        }

        public override void Draw(GameTime gameTime)
        {
            if (hide) return;
            base.Draw(gameTime);
        }

        public override void PostDraw(GameTime gameTime)
        {
            if (mheldItem != null)
            {
                Vector2 mouse = Utilities.Input.GetMousePos();
                mheldItem.Draw(gameTime, new Rectangle(Microsoft.Xna.Framework.Input.Mouse.GetState().Position, new Point(32, 32)), mouse);
            }
        }

        public void OpenInventory()
        {
            hide = false;
            Locked = false;
            Point pos = Utilities.ScreenUtils.Center(new Point(SizeX, SizeY));
            SetPosition(pos.X, pos.Y);
        }

        public void CloseInventory()
        {
            hide = true;
            Locked = true;
            SetPosition(16, 16);
            if (mheldItem != null)
            {

            }
        }
    }
}
