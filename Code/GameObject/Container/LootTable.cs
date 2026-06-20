using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Container
{
    public class LootTable
    {
        public static LootTable exampleOfAFixedLootTable = LootTable.LoadFromFile("implementLater.etf");
        private static Random random = new Random();
        private List<WeightedItemStack> entries;

        public LootTable()
        {
            entries = new List<WeightedItemStack>();
        }

        public static LootTable LoadFromFile(string path)
        {
            return null; //NOT YET IMPLEMENTED
        }

        public Item.ItemStack SelectItem()
        {
            //sum up weights of all items
            //select number
            //create itemstack with random stack size
            //make a method to repeat this multiple times - SelectItems(int count)
            return null;
        }
    }

    public class WeightedItemStack
    {
        public WeightedItemStack(int weight, int id, int stackMin, int stackMax)
        {
            //stuff here
        }
    }
}
