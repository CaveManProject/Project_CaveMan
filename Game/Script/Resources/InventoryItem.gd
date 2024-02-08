class_name InventoryItem extends Resource

@export var amount: int
@export var type: Tile.Type

func _init(_type: Tile.Type):
	type = _type
	amount = 1
	
func get_texture() -> Texture2D:
	return Item.TEXTURE_MAP[type]


