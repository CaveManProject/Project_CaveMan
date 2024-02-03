class_name Inventory extends Resource

signal update

@export var items: Array[Item]

func insert(item: Item):
	var index = items.find(item)
	if index != -1:
		items[index].amount += 1
	else:
		items.append(item)
	update.emit()
