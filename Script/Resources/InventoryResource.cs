using Godot;

namespace Caveman.Resources;

public partial class InventoryResource : Resource
{
    private InventoryItem[] _items;

    [Signal]
    public delegate void UpdateUIEventHandler();

    public void Insert(InventoryItem item)
    {
        for (int i = 0; i < this._items.Length; i++)
        {
            if (this._items[i] == null)
            {
                this._items[i] = item;
               return;
            }
            if(this._items[i] == item)
            {
                this._items[i].amount += item.amount;
                return;
            }
        }
        EmitSignal(SignalName.UpdateUI);
    }

}
   

