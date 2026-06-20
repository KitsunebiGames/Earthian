using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earthian.GameObject.Entity.Entities;
using Earthian.GameObject.World;

namespace Earthian.GameObject.Item.Items
{
    public class ItemPickaxe : ItemTool
    {
        public ItemPickaxe(int id, int tier)
            : base(id, 0, tier)
        {
            SetToolPower((tier + 1) * 50);
            SetSwingTime(15);
            SetScale(2f);
			ForceUnconsumable();
        }

        public ItemPickaxe(int id, string name)
            : base(id, 0, 0)
        {
            this.name = name;
        }
    }
}
