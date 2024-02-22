using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Caveman.Resources
{
    public partial class InventoryResource : Resource
    {
        private readonly static int MAX_SIZE = 3*6;
        public readonly InventoryItem[] items =  new InventoryItem[MAX_SIZE];

        [Signal]
        public delegate void UpdateUIEventHandler();

        public void Insert(InventoryItem item)
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                if(this.items[i] is null){
                    this.items[i] = item;
                    break;
                }
                if (this.items[i] == item)
                {
                    this.items[i].amount += item.amount;
                    break;
                }
            }
            EmitSignal(SignalName.UpdateUI);
        }
    }
}
