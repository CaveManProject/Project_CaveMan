class_name Inventory extends Resource

signal update

@export var items: Array[Item]

func insert(item: Item):
	var itemslots = items.filter(func(_item): return _item == item)
	if !itemslots.is_empty():
		itemslots[0].amount += 1
	else:
		var emptyslots = items.filter(func(_item): return _item == null)
		if !emptyslots.is_empty():
			emptyslots[0].item = item
			emptyslots[0].amount = 1
	update.emit()
