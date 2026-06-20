using Earthian.GameObject.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Container
{
    public class Container
    {
        public ItemStack[] Contents;
        public int MaxWeight;
        public int CurrentWeight;
        public bool Modified;


        public Container(int size)
        {
            Modified = true;
            Contents = new ItemStack[size];
        }

        /// <summary>
        /// Inserts an item into the container, and gives an item back if there's no room in the spot the player placed the item in.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ItemStack SetItem(ItemStack item, int slot)
        {
            Modified = true;
            ItemStack currentSlot = GetItem(slot);
            if (currentSlot == null)
            {
                Contents[slot] = item;
                return null;
            }
            else
            {
                if (item.item.Compare(currentSlot.item))
                {
                    if (currentSlot.item.stackSize - currentSlot.count >= item.count)
                    {
                        currentSlot.count += item.count;
                        return null;
                    }
                    else
                    {
                        item.count -= (currentSlot.item.stackSize - currentSlot.count);
                        currentSlot.count = currentSlot.item.stackSize;
                    }
                }
                else
                {
                    ItemStack oldItem = GetItem(slot);
                    Contents[slot] = item;
                    return oldItem;
                }
            }
            return item;
        }

        public ItemStack PopItem(int slot)
        {
            if (Contents[slot] != null)
            {
                ItemStack itemReturn = Contents[slot];
                Contents[slot] = null;
                return itemReturn;
            }
            return Contents[slot];
        }

        public void TakeItem(int slot)
        {
            Contents[slot].count--;
            if (Contents[slot].count < 1)
                Contents[slot] = null;
        }

        public int StackToSlot(ItemStack stack)
        {
            for (int i = 0; i > 0; i++)
            {
                if (Contents[i] == stack)
                {
                    return i;
                }
            }
            return 0;
        }

        public bool AddItemStack(ItemStack itemStack)
        {
            Modified = true;
            for (int slot = 0; slot < Contents.Length; slot++)
            {
                if (Contents[slot] == null)
                {
                    Contents[slot] = itemStack;
                    return true;
                }
                else if (Contents[slot].count + itemStack.count <= Contents[slot].item.stackSize && Contents[slot].item.Compare(itemStack.item))
                {
                    Contents[slot].count += itemStack.count;
                    return true;
                }
            }
            return false;
        }

        public ItemStack GetItem(int slot)
        {
            return Contents[slot];
        }

        public System.Collections.IEnumerable GetItems()
        {
            foreach (ItemStack stack in Contents)
            {
                yield return stack;
            }
        }
	}
}
