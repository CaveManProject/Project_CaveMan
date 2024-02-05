class_name InventoryItem extends Resource

@export var amount: int
@export var type: Tile.Type

func _init(_type: Tile.Type):
	print(_type)
	type = _type
	amount = 1


