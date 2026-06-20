using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject.Item
{
	public class ItemLookup
	{
		public delegate Item ItemConstructor();

		private static ItemConstructor[] ItemGenerators = new ItemConstructor[32767];
		//() => {return new thing().stuff().moreStuff().CustomName=590484.SetWeight()}

		public static ItemLookup items;

		public ItemLookup()
		{
			items = this;
			RegisterTools();
		}

		public void RegisterItem(int id, ItemConstructor constructor)
		{
			ItemGenerators[id] = constructor;
           	
			//Console.Out.WriteLine("Initialised item id " + id);
		}

		public Item ConstructItem(int id)
		{
			if (id > 32767)
			{
				return Block.Blocks.Blocks.InstantiateBlock(id - 32768, false).GetThisAsItem();
			}
			ItemConstructor d = ItemGenerators[id];
			if (d != null)
			{
				Item i = d();
				Console.WriteLine("Constructed item: " + i.name + " with id " + i.id);
				return i;
			}
			return null;
		}

		public Item ConstructItemBlock(int id)
		{
			return Block.Blocks.Blocks.InstantiateBlock(id, false).GetThisAsItem();
		}

		internal static void RegisterTools()
		{
			int id;
			int tools = 5;
			foreach (Items.ToolSettings tool in Items.ItemTool.GetToolTypes())
			{
				foreach (Items.ItemTool.EToolTier type in Enum.GetValues(typeof(Items.ItemTool.EToolTier)))
				{
					id = (tool.ID + (tool.ID * tools)) + (int)type;
					Console.WriteLine("[Tool Register] Registered tool: " + type + " " + tool.GetName() + " with id " + id);
					ItemLookup.items.RegisterItem(id, () =>
						{
							return new Items.ItemTool(id, tool.ID, (int)type);
						});
				}
			}
		}
	}
}
