using Caveman.Player;
using Caveman.Resources;
using Godot;
using System;

namespace Caveman.Inventory;

public partial class InventoryUISlot : Control
{
	private Sprite2D _itemVisual;
	private Label _amountText;

	public override void _Ready()
	{
		this._itemVisual = this.GetNode<Sprite2D>("CenterContainer/Panel/item_display");
		this._amountText = this.GetNode<Label>("CenterContainer/Panel/Label");
	}

	public void Update(InventoryItem item)
	{
		if (item == null)
		{
			this._itemVisual.Visible = false;
		}
		else
		{
			this._itemVisual.Visible = true;
			this._itemVisual.Texture = item.GetTexture();
			if (item.amount > 1)
			{
				this._amountText.Visible = true;
				this._amountText.Text = item.amount.ToString();
			}
		}
	}
}
