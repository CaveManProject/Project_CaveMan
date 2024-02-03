class_name InventoryUISlot extends Panel

@onready var item_visual: Sprite2D = $CenterContainer/Panel/item_display
@onready var amount_text: Label = $CenterContainer/Panel/Label

func update(item: Item):
	if !item:
		item_visual.visible = false
	else:
		item_visual.visible = true
		item_visual.texture = item.texture
		if item.amount > 1:
			amount_text.visible = true
			amount_text.text = str(item.amount)
