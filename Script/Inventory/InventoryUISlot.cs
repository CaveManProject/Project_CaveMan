using Caveman.Resources;
using Godot;

namespace Caveman.Inventory
{
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
			
			this._itemVisual.Visible = true;
			this._itemVisual.Texture = item.GetTexture();
			if (item.amount > 1)
			{
				this._amountText.Visible = true;
				this._amountText.Text = item.amount.ToString();
			}
			
		}

		public void HideSlot()
		{
			GD.Print("Hiding slot");
			this._itemVisual.Visible = false;
		}
	}
}
