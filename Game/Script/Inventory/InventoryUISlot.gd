class_name InventoryUISlot extends Panel

@onready var item_visual: Sprite2D = $CenterContainer/Panel/item_display
@onready var amount_text: Label = $CenterContainer/Panel/Label

func update(item: InventoryItem):
	if !item:
		item_visual.visible = false
	else:
		item_visual.visible = true
		item_visual.texture = Item.get_texture(item.type)
		if item.amount > 1:
			amount_text.visible = true
			amount_text.text = str(item.amount)
