using Caveman.Resources;
using Godot;

namespace Caveman.Inventory
{
	public partial class InventoryUI : Control
	{
		private InventoryResource _inventoryResource = GD.Load<InventoryResource>("res://Data/inventory.tres");
		private GridContainer _itemSlots;

		public override void _Ready()
		{
			this.ZIndex = 100;
			this._inventoryResource.UpdateUI += UpdateSlots;
			this._itemSlots = this.GetNode<GridContainer>("NinePatchRect/GridContainer");
			UpdateSlots();
			Close();
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed("i"))
			{
				if (this.Visible)
				{
					Close();
				}
				else
				{
					Open();
				}
			}
		}

		private void UpdateSlots() {
			for (int i = 0; i < Mathf.Min(this._inventoryResource.items.Length, this._itemSlots.GetChildCount()); i++)
			{
				var slot = this._itemSlots.GetChild<InventoryUISlot>(i);
                if(this._inventoryResource.items[i] is null){
                    slot.HideSlot();
                }else{
                    slot.Update(this._inventoryResource.items[i]);
                }
				
			}

		 }

		public void Close()
		{
			this.Visible = false;
		}

		public void Open()
		{
			this.Visible = true;
		}
	}
}
