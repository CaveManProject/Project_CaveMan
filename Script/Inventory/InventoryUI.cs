using Caveman.Player;
using Caveman.Resources;
using Godot;
using System;

namespace Caveman.Inventory;

public partial class InventoryUI : Control
{
	private InventoryResource _inventoryResource;
	private Node2D _itemSlots;

	public override void _Ready()
	{
		this.ZIndex = 100;
		this._inventoryResource = this.GetNode<InventoryResource>("InventoryResource");
		this._inventoryResource.UpdateUI += UpdateSlots;
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

	private void UpdateSlots(){

	}

	public void Close(){
		this.Visible = false;
	}

	public void Open(){
		this.Visible = true;
	}
}
