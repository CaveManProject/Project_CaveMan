class_name Inventory extends Resource

signal update

@export var items: Array[InventoryItem]

func insert(item: InventoryItem):
	var filtered = items.filter(func(_item): return _item.type == item.type)
	if filtered.size() > 0:
		filtered[0].amount += 1
	else:
		items.append(item)
	update.emit()
