using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class Inventory
    {
        private List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }

        public bool AddItem(Item item)
        {
            items.Add(item);
            return true;
        }

        public bool RemoveItem(Item item)
        {
            items.Remove(item);
            return true;
        }

        public bool HasItem(Item item)
        {
            return items.Contains(item);
        }

        public List<Item> GetAllItems()
        {
            return items;
        }
    }
}
