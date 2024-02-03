class_name Inventory extends Resource

signal update

@export var items: Array[InventoryItem]

func insert(item: InventoryItem):
	var index = items.find(item)
	if index != -1:
		items[index].amount += 1
	else:
		items.append(item)
	update.emit()
